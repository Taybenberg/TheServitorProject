using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager : IClanActivities
    {
        public async Task<UserPartnersContainer> GetUserPartnersAsync(ulong userID, DateTime? period = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var user = await activitiesDB.GetUserWithActivitiesAndOtherUserStatsAsync(userID, period);

            if (user is null)
                return null;

            var allActivities = user.Characters.SelectMany(x => x.ActivityUserStats.Select(y => y.Activity)).Distinct();
            var coopActivities = allActivities.Where(x  => x.ActivityUserStats.Any(y => !user.Characters.Any(c => c.CharacterID == y.CharacterID)));

            var allUsers = await activitiesDB.GetUsersWithCharactersAsync();
            var coopUsers = allUsers.Where(x => x.UserID != user.UserID);

            ConcurrentDictionary<long, ConcurrentDictionary<int, int>> activityCounter = new();

            foreach (var partner in coopUsers)
                activityCounter.TryAdd(partner.UserID, new());

            var chunks = coopActivities.Chunk(coopActivities.Count() / 8 + 1);
            
            var tasks = chunks.Select(x => Task.Run(() =>
            {
                foreach (var activity in x)
                    foreach (var partner in coopUsers)
                        if (activity.ActivityUserStats.Any(y => partner.Characters.Any(c => c.CharacterID == y.CharacterID)))
                            activityCounter[partner.UserID].AddOrUpdate(activity.ActivityType, 1, (k, v) => v + 1);
            }));

            var allCount = allActivities.Count();
            var coopCount = coopActivities.Count();

            var userNames = coopUsers.ToDictionary(x => x.UserID, x => x.UserName);

            await Task.WhenAll(tasks);

            var partners = activityCounter.ToDictionary(
                x => x.Key, 
                x => x.Value.Sum(y => y.Value))
                .Where(x => x.Value > 0)
                .OrderByDescending(x => x.Value);

            ConcurrentBag<(string, int[])> topPartnersTypeCounters = new();
            var containerTasks = partners.Take(8)
                .Select(x => Task.Run(() =>
                {
                    var container = new ModeCountersContainer
                    {
                        Counters = activityCounter[x.Key]
                            .Select(y => new ModeCounter
                            {
                                ActivityMode = (DestinyActivityModeType)y.Key,
                                Count = y.Value
                            })
                    };

                    topPartnersTypeCounters.Add((userNames[x.Key], container.TypeCounters));
                }));

            var partnersCounters = partners
                .Select(x => new PartnersCounter
                {
                    UserName = userNames[x.Key],
                    Count = x.Value
                });

            await Task.WhenAll(containerTasks);

            return new UserPartnersContainer
            {
                UserName = user.UserName,
                TotalCount = allCount,
                CoopCount = coopCount,
                Partners = partnersCounters,
                TopPartners = topPartnersTypeCounters
            };
        }
    }
}
