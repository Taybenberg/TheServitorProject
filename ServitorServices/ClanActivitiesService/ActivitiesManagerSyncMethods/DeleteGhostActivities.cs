using ClanActivitiesDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        private async Task DeleteGhostActivitiesAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Deleting ghost Activities");

            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var activities = await activitiesDB.GetActivitiesWithActivityUserStatsAsync(null);

            var ghostActivities = activities.Where(x => !x.ActivityUserStats.Any());

            await activitiesDB.SyncActivitiesAsync(ghostActivities, null, null);

            _logger.LogInformation($"{DateTime.Now} Ghost Activities deleted");
        }
    }
}
