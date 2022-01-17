//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ClanActivitiesService
//{
//    internal class Class1
//    {
//        public async Task SyncUsersAsync()
//        {
//            _logger.LogInformation($"{DateTime.Now} Syncing Users");

//            using var scope = _scopeFactory.CreateScope();

//            var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

//            ConcurrentBag<IEnumerable<BungieNetApi.Entities.User>> clanUsersCollection = new();

//            Parallel.ForEach(_configuration.GetSection("Destiny2:ClanIDs").Get<HashSet<long>>(), (clanID) =>
//            {
//                clanUsersCollection.Add(apiClient.GetClan(clanID).GetUsersAsync().Result);
//            });

//            var clanUsers = clanUsersCollection.SelectMany(x => x).ToDictionary(x => x.MembershipID, x => x);

//            var dbUsers = await _context.Users.Include(c => c.Characters).ToDictionaryAsync(x => x.UserID, x => x);

//            ConcurrentBag<User> newUsers = new();
//            ConcurrentBag<User> updUsers = new();

//            ConcurrentBag<Character> diffChars = new();
//            ConcurrentBag<Character> newChars = new();
//            ConcurrentBag<Character> updChars = new();

//            var diffDbUsers = dbUsers.Where(x => !clanUsers.ContainsKey(x.Key)).Select(x => x.Value);

//            Parallel.ForEach(clanUsers, (usr) =>
//            {
//                var dbUsr = dbUsers.GetValueOrDefault(usr.Key);

//                if (dbUsr is null)
//                {
//                    newUsers.Add(new User
//                    {
//                        UserID = usr.Value.MembershipID,
//                        ClanID = usr.Value.ClanID,
//                        UserName = usr.Value.BungieName,
//                        DateLastPlayed = usr.Value.DateLastPlayed,
//                        ClanJoinDate = usr.Value.ClanJoinDate,
//                        MembershipType = usr.Value.MembershipType,
//                        Characters = usr.Value.Characters.Select(chr =>
//                        new Character
//                        {
//                            CharacterID = chr.CharacterID,
//                            DateLastPlayed = chr.DateLastPlayed,
//                            Class = chr.Class,
//                            Race = chr.Race,
//                            Gender = chr.Gender,
//                            UserID = chr.MembershipID
//                        }).ToList()
//                    });
//                }
//                else if (dbUsr.DateLastPlayed < usr.Value.DateLastPlayed)
//                {
//                    dbUsr.ClanID = usr.Value.ClanID;
//                    dbUsr.UserName = usr.Value.BungieName;
//                    dbUsr.DateLastPlayed = usr.Value.DateLastPlayed;
//                    dbUsr.MembershipType = usr.Value.MembershipType;

//                    updUsers.Add(dbUsr);

//                    foreach (var diff in dbUsr.Characters.Where(x => !usr.Value.Characters.Any(y => y.CharacterID == x.CharacterID)))
//                    {
//                        diffChars.Add(diff);
//                    }

//                    foreach (var chr in usr.Value.Characters)
//                    {
//                        var dbChr = dbUsr.Characters.FirstOrDefault(x => x.CharacterID == chr.CharacterID);

//                        if (dbChr is null)
//                        {
//                            newChars.Add(new Character
//                            {
//                                CharacterID = chr.CharacterID,
//                                DateLastPlayed = chr.DateLastPlayed,
//                                Class = chr.Class,
//                                Race = chr.Race,
//                                Gender = chr.Gender,
//                                UserID = chr.MembershipID
//                            });
//                        }
//                        else
//                        {
//                            dbChr.DateLastPlayed = chr.DateLastPlayed;
//                            dbChr.Class = chr.Class;
//                            dbChr.Race = chr.Race;
//                            dbChr.Gender = chr.Gender;

//                            updChars.Add(dbChr);
//                        }
//                    }
//                }
//            });

//            _context.Users.RemoveRange(diffDbUsers);
//            _context.Users.AddRange(newUsers);
//            _context.Users.UpdateRange(updUsers);

//            _context.Characters.RemoveRange(diffChars);
//            _context.Characters.AddRange(newChars);
//            _context.Characters.UpdateRange(updChars);

//            await _context.SaveChangesAsync();

//            _logger.LogInformation($"{DateTime.Now} Users synced");
//        }

//        public async Task SyncActivitiesAsync()
//        {
//            _logger.LogInformation($"{DateTime.Now} Syncing Activities");

//            using var scope = _scopeFactory.CreateScope();

//            var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

//            var factory = apiClient.EntityFactory;

//            DateTime date = DateTime.Now.AddDays(-7);

//            var lastKnownActivities = await _context.Characters.Include(x => x.User).Select(c => new
//            {
//                Character = c,
//                Activity = c.ActivityUserStats.OrderByDescending(a => a.Activity.Period).FirstOrDefault().Activity
//            }).ToListAsync();

//            var userIDs = lastKnownActivities.Select(x => x.Character.UserID).ToHashSet();
//            var charIDs = lastKnownActivities.Select(x => x.Character.CharacterID).ToHashSet();

//            ConcurrentDictionary<long, BungieNetApi.Entities.Activity> newActivitiesDictionary = new();

//            Parallel.ForEach(lastKnownActivities.Where(x => x.Character.DateLastPlayed > date), (last) =>
//            {
//                Func<BungieNetApi.Entities.Activity, bool> newActivitiesFilter =
//                    x => x.Period > last.Character.User.ClanJoinDate && x.Period > (last?.Activity?.Period ?? date);

//                var rawChar = factory.GetCharacter(last.Character.CharacterID, last.Character.UserID, last.Character.User.MembershipType);

//                if (rawChar.GetActivitiesAsync(1, 0).Result.Where(newActivitiesFilter).Any())
//                {
//                    IEnumerable<BungieNetApi.Entities.Activity> newActivitiesBuffer;

//                    int page = 0, count = 25;

//                    while ((newActivitiesBuffer = rawChar.GetActivitiesAsync(count, page++).Result.Where(newActivitiesFilter)).Any())
//                    {
//                        foreach (var act in newActivitiesBuffer)
//                            if (!newActivitiesDictionary.TryAdd(act.InstanceID, act))
//                                newActivitiesDictionary[act.InstanceID].MergeUserStats(act.UserStats);
//                    }
//                }
//            });

//            var nfIDs = _configuration.GetSection("Destiny2:NoMatchmakingNightfalls").Get<HashSet<long>>();

//            ConcurrentBag<Activity> newActivities = new();

//            Parallel.ForEach(newActivitiesDictionary, (act) =>
//            {
//                int? suspicionIndex = null;

//                var clanmateStats = act.Value.UserStats;

//                if (act.Value.ActivityType is ActivityType.Raid or ActivityType.Dungeon or ActivityType.ScoredNightfall)
//                {
//                    var rawAct = factory.GetActivity(act.Key);

//                    clanmateStats = rawAct.UserStats.Where(x => userIDs.Contains(x.MembershipID));

//                    if (rawAct.UserStats.Count() > clanmateStats.Count())
//                    {
//                        if (act.Value.ActivityType == ActivityType.ScoredNightfall)
//                        {
//                            if (nfIDs.Contains(act.Value.ReferenceID) || clanmateStats.First().TeamScore > 150000)
//                                suspicionIndex = rawAct.UserStats.Count() - clanmateStats.Count();
//                        }
//                        else
//                            suspicionIndex = rawAct.UserStats.Count() - clanmateStats.Count();

//                        if (suspicionIndex <= 0)
//                            suspicionIndex = null;
//                    }
//                }

//                newActivities.Add(new Activity
//                {
//                    ActivityID = act.Key,
//                    Period = act.Value.Period,
//                    ActivityType = act.Value.ActivityType,
//                    SuspicionIndex = suspicionIndex,
//                    ReferenceHash = act.Value.ReferenceID,
//                    ActivityHash = act.Value.DirectorActivityHash,
//                    ActivityUserStats = clanmateStats
//                    .Where(x => charIDs.Contains(x.CharacterID)).Select(y =>
//                    new ActivityUserStats
//                    {
//                        ActivityID = act.Key,
//                        CharacterID = y.CharacterID,
//                        ActivityDurationSeconds = y.ActivityDurationSeconds,
//                        Completed = y.Completed,
//                        CompletionReasonValue = y.CompletionReasonValue,
//                        CompletionReasonDisplayValue = y.CompletionReasonDisplayValue,
//                        StandingValue = y.StandingValue,
//                        StandingDisplayValue = y.StandingDisplayValue
//                    }).ToList()
//                });
//            });

//            var lastActivitiesIds = _context.Activities.Where(x => x.Period > date).Select(y => y.ActivityID).ToHashSet();

//            _context.Activities.AddRange(newActivities.Where(x => !lastActivitiesIds.Contains(x.ActivityID)));

//            await _context.SaveChangesAsync();

//            _logger.LogInformation($"{DateTime.Now} Activities synced");
//        }
//    }
//}
