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
        private async Task SyncActivityDefinitionsAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing ActivityDefinitions");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            _logger.LogInformation($"{DateTime.Now} ActivityDefinitions synced");
        }
    }
}
