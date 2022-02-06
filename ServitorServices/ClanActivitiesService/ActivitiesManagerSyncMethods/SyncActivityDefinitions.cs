using BungieSharper.Client;
using ClanActivitiesDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        private async Task SyncActivityDefinitionsAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing ActivityDefinitions");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var activities = await activitiesDB.GetActivitiesAsync(null);

            var activityReferenceIDs = activities.Select(x => x.ReferenceHash).Distinct();
            var activityDirectorHashes = activities.Select(x => x.ActivityHash).Distinct();

            var uniqueDirectorHashes = activityDirectorHashes.Except(activityReferenceIDs);

            foreach (var u in uniqueDirectorHashes)
            {
                var entity = await apiClient.Api.Destiny2_GetDestinyEntityDefinition("DestinyActivityDefinition", (uint)u);
            }

            _logger.LogInformation($"{DateTime.Now} ActivityDefinitions synced");
        }
    }
}
