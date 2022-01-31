using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager : IClanActivities
    {
        public async Task<ModeCountersContainer> GetClanActivitiesAsync(DateTime? period = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var activities = await activitiesDB.GetActivitiesAsync(period);

            ConcurrentDictionary<int, int> activityCounter = new();

            var chunks = activities.Chunk(activities.Count() / 8 + 1);

            var tasks = chunks.Select(x => Task.Run(() =>
            {
                foreach (var activity in x)
                    activityCounter.AddOrUpdate(activity.ActivityType, 1, (k, v) => v + 1);
            }));

            await Task.WhenAll(tasks);

            return new ModeCountersContainer
            {
                Counters = activityCounter.Select(x => new ModeCounter
                {
                    ActivityMode = (DestinyActivityModeType)x.Key,
                    Count = x.Value
                }).OrderByDescending(x => x.Count)
            };
        }
    }
}
