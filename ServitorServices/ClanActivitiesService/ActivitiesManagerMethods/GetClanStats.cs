using BungieSharper.Client;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using Microsoft.Extensions.DependencyInjection;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task<IEnumerable<ClanStat>> GetClanStatsAsync(ulong userID, DestinyActivityModeType activityType)
        {
            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var user = await activitiesDB.GetUserByDiscordIdAsync(userID);

            if (user is null)
                return null;

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var clanStats = await apiClient.Api.Destiny2_GetClanAggregateStats(user.ClanID, ((int)activityType).ToString());

            return clanStats.Select(x => new ClanStat
            {
                StatName = x.StatId,
                Value = x.Value.Basic.DisplayValue
            });
        }
    }
}
