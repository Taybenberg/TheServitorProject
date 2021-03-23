using BungieNetApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database
{
    public partial class ClanDatabase
    {
        public async Task SyncActivitiesAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Activities");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            DateTime date = DateTime.Now.AddDays(-7);

            ConcurrentDictionary<long, Activity> newActivitiesDictionary = new();

            var lastKnownActivities = await Characters.Include(x => x.User).Select(c => new
            {
                Character = c,
                Activity = c.ActivityUserStats.OrderByDescending(a => a.Activity.Period).FirstOrDefault().Activity
            }).ToListAsync();

            var userIDs = lastKnownActivities.Select(x => x.Character.UserID).ToHashSet();
            var charIDs = lastKnownActivities.Select(x => x.Character.CharacterID).ToHashSet();

            Parallel.ForEach(lastKnownActivities, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (last) =>
            {
                Func<BungieNetApi.Activity, bool> newActivitiesFilter;

                if (last.Activity is not null)
                    newActivitiesFilter = x => x.Period > last.Character.User.ClanJoinDate && x.Period > last.Activity.Period;
                else
                    newActivitiesFilter = x => x.Period > last.Character.User.ClanJoinDate && x.Period > date;

                if (apiClient.GetUserActivitiesAsync(last.Character.User.MembershipType, last.Character.UserID, last.Character.CharacterID, 1, 0)
                .Result.Select(x => x.Value).Where(newActivitiesFilter).Any())
                {
                    int page = 0, count = 25;

                    IEnumerable<BungieNetApi.Activity> newActivitiesBuffer;

                    while ((newActivitiesBuffer = apiClient.GetUserActivitiesAsync(last.Character.User.MembershipType, last.Character.UserID, last.Character.CharacterID, count, page++)
                    .Result.Select(x => x.Value).Where(newActivitiesFilter)).Any())
                    {
                        Parallel.ForEach(newActivitiesBuffer.Where(x => !newActivitiesDictionary.ContainsKey(x.InstanceId)), (act) =>
                        {
                            int? suspicionIndex = null;

                            var clanmateStats = act.ActivityUserStats.Where(x => userIDs.Contains(x.MembershipId));

                            if (act.ActivityUserStats.Count() > clanmateStats.Count() &&
                            act.ActivityType switch
                            {
                                ActivityType.TrialsOfOsiris or
                                ActivityType.Raid or
                                ActivityType.Dungeon => true,
                                ActivityType.ScoredNightfall => clanmateStats.First().TeamScore > 100000,
                                _ => false
                            })
                            {
                                if (act.ActivityType == ActivityType.TrialsOfOsiris)
                                    suspicionIndex = act.ActivityUserStats.Select(x => x.MembershipId).Distinct().Count() - clanmateStats.Count() - 3;
                                else
                                    suspicionIndex = act.ActivityUserStats.Count() - clanmateStats.Count();

                                if (suspicionIndex <= 0)
                                    suspicionIndex = null;
                            }

                            newActivitiesDictionary.TryAdd(act.InstanceId, new Activity
                            {
                                ActivityID = act.InstanceId,
                                Period = act.Period,
                                ActivityType = act.ActivityType,
                                SuspicionIndex = suspicionIndex,
                                ActivityUserStats = clanmateStats
                                .Where(x => charIDs.Contains(x.CharacterId)).Select(y =>
                                new ActivityUserStats
                                {
                                    ActivityID = act.InstanceId,
                                    CharacterID = y.CharacterId,
                                    ActivityDurationSeconds = y.ActivityDurationSeconds,
                                    Completed = y.Completed,
                                    CompletionReasonValue = y.CompletionReasonValue,
                                    CompletionReasonDisplayValue = y.CompletionReasonDisplayValue,
                                    StandingValue = y.StandingValue,
                                    StandingDisplayValue = y.StandingDisplayValue
                                }).ToList()
                            });
                        });
                    }
                }
            });

            var lastActivitiesIds = Activities.Where(x => x.Period > date).Select(y => y.ActivityID).ToHashSet();

            Activities.AddRange(newActivitiesDictionary.Where(x => !lastActivitiesIds.Contains(x.Key)).Select(x => x.Value));

            await SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now} Activities synced");
        }
    }
}
