using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BungieNetApi;

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

            var lastKnownActivities = await Characters.Include(x => x.User).Include(y => y.ActivityUserStats).ThenInclude(z => z.Activity)
                .Select(c => new { Character = c, ActivityUserStats = c.ActivityUserStats.OrderByDescending(a => a.Activity.Period).FirstOrDefault() }).ToListAsync();

            Parallel.ForEach(lastKnownActivities, (last) =>
            {
                Func<BungieNetApi.Activity, bool> newActivitiesFilter;

                if (last.ActivityUserStats is not null)
                    newActivitiesFilter = x => x.Period > last.Character.User.ClanJoinDate && x.Period > last.ActivityUserStats.Activity.Period;
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
                        foreach(var act in newActivitiesBuffer.Where(x => !newActivitiesDictionary.ContainsKey(x.InstanceId)))
                        {
                            int? suspicionIndex = null;

                            var clanmateStats = act.ActivityUserStats.Where(x => lastKnownActivities.Any(y => y.Character.UserID == x.MembershipId));

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
                                suspicionIndex = act.ActivityUserStats.Count() - clanmateStats.Count() - (act.ActivityType == ActivityType.TrialsOfOsiris ? 3 : 0);

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
                                .Where(x => lastKnownActivities.Any(y => y.Character.CharacterID == x.CharacterId)).Select(y =>
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
                        }
                    }
                }
            });

            Activities.AddRange(newActivitiesDictionary.Select(x => x.Value).OrderBy(y => y.Period));

            await SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now} Activities synced");
        }
    }
}
