using BungieNetApi;
using BungieNetApi.Enums;
using Database.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database
{
    public partial class ClanUoW
    {
        public async Task SyncActivitiesAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Activities");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var factory = apiClient.EntityFactory;

            DateTime date = DateTime.Now.AddDays(-7);

            var lastKnownActivities = await _context.Characters.Include(x => x.User).Select(c => new
            {
                Character = c,
                Activity = c.ActivityUserStats.OrderByDescending(a => a.Activity.Period).FirstOrDefault().Activity
            }).ToListAsync();

            var userIDs = lastKnownActivities.Select(x => x.Character.UserID).ToHashSet();
            var charIDs = lastKnownActivities.Select(x => x.Character.CharacterID).ToHashSet();

            ConcurrentDictionary<long, BungieNetApi.Entities.Activity> newActivitiesDictionary = new();

            Parallel.ForEach(lastKnownActivities.Where(x => x.Character.DateLastPlayed > date), (last) =>
            {
                Func<BungieNetApi.Entities.Activity, bool> newActivitiesFilter =
                    x => x.Period > last.Character.User.ClanJoinDate && x.Period > (last?.Activity?.Period ?? date);

                var rawChar = factory.GetCharacter(last.Character.CharacterID, last.Character.UserID, last.Character.User.MembershipType);

                if (rawChar.GetActivitiesAsync(1, 0).Result.Where(newActivitiesFilter).Any())
                {
                    IEnumerable<BungieNetApi.Entities.Activity> newActivitiesBuffer;

                    int page = 0, count = 25;

                    while ((newActivitiesBuffer = rawChar.GetActivitiesAsync(count, page++).Result.Where(newActivitiesFilter)).Any())
                    {
                        foreach (var act in newActivitiesBuffer)
                            if (!newActivitiesDictionary.TryAdd(act.InstanceID, act))
                                newActivitiesDictionary[act.InstanceID].MergeUserStats(act.UserStats);
                    }
                }
            });

            var nfIDs = _configuration.GetSection("Destiny2:NoMatchmakingNightfalls").Get<HashSet<long>>();

            ConcurrentBag<Activity> newActivities = new();

            Parallel.ForEach(newActivitiesDictionary, (act) =>
            {
                int? suspicionIndex = null;

                var clanmateStats = act.Value.UserStats;

                if (act.Value.ActivityType is ActivityType.Raid or ActivityType.Dungeon or ActivityType.ScoredNightfall)
                {
                    var rawAct = factory.GetActivity(act.Key);

                    clanmateStats = rawAct.UserStats.Where(x => userIDs.Contains(x.MembershipID));

                    if (rawAct.UserStats.Count() > clanmateStats.Count())
                    {
                        if (act.Value.ActivityType == ActivityType.ScoredNightfall)
                        {
                            if (nfIDs.Contains(act.Value.ReferenceID) || clanmateStats.First().TeamScore > 150000)
                                suspicionIndex = rawAct.UserStats.Count() - clanmateStats.Count();
                        }
                        else
                            suspicionIndex = rawAct.UserStats.Count() - clanmateStats.Count();

                        if (suspicionIndex <= 0)
                            suspicionIndex = null;
                    }
                }

                newActivities.Add(new Activity
                {
                    ActivityID = act.Key,
                    Period = act.Value.Period,
                    ActivityType = act.Value.ActivityType,
                    SuspicionIndex = suspicionIndex,
                    ReferenceHash = act.Value.ReferenceID,
                    ActivityHash = act.Value.DirectorActivityHash,
                    ActivityUserStats = clanmateStats
                    .Where(x => charIDs.Contains(x.CharacterID)).Select(y =>
                    new ActivityUserStats
                    {
                        ActivityID = act.Key,
                        CharacterID = y.CharacterID,
                        ActivityDurationSeconds = y.ActivityDurationSeconds,
                        Completed = y.Completed,
                        CompletionReasonValue = y.CompletionReasonValue,
                        CompletionReasonDisplayValue = y.CompletionReasonDisplayValue,
                        StandingValue = y.StandingValue,
                        StandingDisplayValue = y.StandingDisplayValue
                    }).ToList()
                });
            });

            var lastActivitiesIds = _context.Activities.Where(x => x.Period > date).Select(y => y.ActivityID).ToHashSet();

            _context.Activities.AddRange(newActivities.Where(x => !lastActivitiesIds.Contains(x.ActivityID)));

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now} Activities synced");
        }
    }
}
