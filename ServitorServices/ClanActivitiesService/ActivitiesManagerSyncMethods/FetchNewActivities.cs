using BungieSharper.Client;
using BungieSharper.Entities;
using ClanActivitiesDatabase;
using ClanActivitiesDatabase.ORM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        private async Task FetchNewActivitiesAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Fetching new Activities");

            var date = DateTime.UtcNow.AddDays(-7);

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var users = await activitiesDB.GetUsersWithCharactersAndLastActivityAsync();

            var lastCharactersWithActivities = users.SelectMany(x => x.Characters.Select(y => (y, y.ActivityUserStats.FirstOrDefault()?.Activity)));

            var recentCharactersWithActivities = lastCharactersWithActivities.Where(x => x.y.DateLastPlayed > date);

            ConcurrentDictionary<long, Activity> newActivitiesDictionary = new();
            ConcurrentDictionary<long, IEnumerable<ActivityUserStats>> newUserStatsDictionary = new();

            var chunks = recentCharactersWithActivities.Chunk(recentCharactersWithActivities.Count() / 8 + 1);

            var tasks = chunks.Select(x => Task.Run(async () =>
            {
                foreach (var pair in x)
                    try
                    {
                        int page = 0, count = 25;

                        while (true)
                        {
                            var history = await apiClient.Api.Destiny2_GetActivityHistory(pair.y.CharacterID, pair.y.UserID, (BungieMembershipType)pair.y.User.MembershipType, page: page, count: count);
                            page++;

                            var newActivities = history.Activities.Where(x => x.Period > pair.y.User.ClanJoinDate && x.Period > (pair.Activity?.Period ?? date));

                            if (!newActivities.Any())
                                break;

                            foreach (var act in newActivities)
                            {
                                if (!newActivitiesDictionary.ContainsKey(act.ActivityDetails.InstanceId))
                                    newActivitiesDictionary.TryAdd(act.ActivityDetails.InstanceId, new Activity
                                    {
                                        ActivityID = act.ActivityDetails.InstanceId,
                                        Period = act.Period,
                                        ActivityType = (int)act.ActivityDetails.Mode,
                                        ReferenceHash = act.ActivityDetails.ReferenceId,
                                        ActivityHash = act.ActivityDetails.DirectorActivityHash
                                    });

                                var userStats = new ActivityUserStats[]
                                {
                                    new ActivityUserStats
                                    {
                                        ActivityID = act.ActivityDetails.InstanceId,
                                        CharacterID = pair.y.CharacterID,
                                        ActivityDurationSeconds = (float)act.Values["activityDurationSeconds"].Basic.Value,
                                        Completed = act.Values["completed"].Basic.Value > 0,
                                        CompletionReasonValue = (float)act.Values["completionReason"].Basic.Value,
                                        CompletionReasonDisplayValue = act.Values["completionReason"].Basic.DisplayValue,
                                        StandingValue = act.Values.ContainsKey("standing") ? (float)act.Values["standing"].Basic.Value : -1.0f,
                                        StandingDisplayValue = act.Values.ContainsKey("standing") ? act.Values["standing"].Basic.DisplayValue : "Unknown"
                                    }
                                };

                                newUserStatsDictionary.AddOrUpdate(act.ActivityDetails.InstanceId, userStats, (k, v) => v.Concat(userStats));
                            }
                        }
                    }
                    catch { }
            }));

            var userIDs = users.Select(x => x.UserID).ToHashSet();
            var characterIDs = users.SelectMany(x => x.Characters.Select(y => y.CharacterID)).ToHashSet();

            var lastDBActivities = await activitiesDB.GetActivitiesAsync(date);
            var lastDBActivitiesIDs = lastDBActivities.Select(x => x.ActivityID).ToHashSet();

            await Task.WhenAll(tasks);

            var activitiesToAdd = newActivitiesDictionary.Where(x => !lastDBActivitiesIDs.Contains(x.Key));

            foreach (var act in activitiesToAdd)
                act.Value.ActivityUserStats = newUserStatsDictionary[act.Key].ToList();

            await activitiesDB.SyncActivitiesAsync(null, null, activitiesToAdd.Select(x => x.Value));

            _logger.LogInformation($"{DateTime.Now} New Activities fetched");
        }
    }
}
