using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task<UserActivitiesContainer> GetUserActivitiesAsync(ulong userID, DateTime? period = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var user = await activitiesDB.GetUserWithActivitiesAsync(userID, period);

            if (user is null)
                return null;

            var activities = user.Characters.SelectMany(c => c.ActivityUserStats.Select(z => z.Activity)).Distinct();

            ConcurrentDictionary<int, int> activityCounter = new();

            var chunks = activities.Chunk(activities.Count() / 8 + 1);

            var tasks = chunks.Select(x => Task.Run(() =>
            {
                foreach (var activity in x)
                    activityCounter.AddOrUpdate(activity.ActivityType, 1, (k, v) => v + 1);
            }));

            var classes = user.Characters.Select(x => new ClassCounter
            {
                Class = (DestinyClass)x.Class,
                Count = x.ActivityUserStats.Count
            }).OrderByDescending(x => x.Count);

            await Task.WhenAll(tasks);

            var modeCounters = new ModeCountersContainer
            {
                Counters = activityCounter.Select(x => new ModeCounter
                {
                    ActivityMode = (DestinyActivityModeType)x.Key,
                    Count = x.Value
                }).OrderByDescending(x => x.Count)
            };

            return new UserActivitiesContainer
            {
                UserName = user.UserName,
                ClassCounters = classes,
                ModeCounters = modeCounters
            };
        }
    }
}
