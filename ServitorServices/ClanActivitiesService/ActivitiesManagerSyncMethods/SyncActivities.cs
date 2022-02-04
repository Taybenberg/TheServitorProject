using BungieSharper.Client;
using ClanActivitiesDatabase;
using ClanActivitiesDatabase.ORM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task SyncActivitiesAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Activities");

            var date = DateTime.UtcNow.AddDays(-7);

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var users = await activitiesDB.GetUsersWithCharactersAndLastActivityAsync();

            var userIDs = users.Select(x => x.UserID).ToHashSet();
            var characterIDs = users.SelectMany(x => x.Characters.Select(y => y.CharacterID)).ToHashSet();

            var lastCharactersWithActivities = users.SelectMany(x => x.Characters.Select(y => (y, y.ActivityUserStats.FirstOrDefault()?.Activity)));

            var recentCharactersWithActivities = lastCharactersWithActivities.Where(x => x.y.DateLastPlayed > date);

            ConcurrentDictionary<long, Activity> newActivitiesDictionary = new();

            var chunks = recentCharactersWithActivities.Chunk(recentCharactersWithActivities.Count() / 8 + 1);

            var tasks = chunks.Select(x => Task.Run(async () =>
            {

            }));

            var lastDBActivities = await activitiesDB.GetActivitiesAsync(date);
            var lastDBActivitiesIDs = lastDBActivities.Select(x => x.ActivityID).ToHashSet();

            await Task.WhenAll(tasks);

            await activitiesDB.SyncActivitiesAsync(newActivitiesDictionary.Where(x => !lastDBActivitiesIDs.Contains(x.Key)).Select(x => x.Value));

            _logger.LogInformation($"{DateTime.Now} Activities synced");
        }
    }
}

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