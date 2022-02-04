using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager : IClanActivities
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly long[] _clanIDs;

        public ClanActivitiesManager(ILogger<ClanActivitiesManager> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            _clanIDs = configuration.GetSection("Destiny2:ClanIDs").Get<long[]>();
        }

        public async Task SyncDatabaseAsync()
        {
            await SyncUsersAsync();

            await SyncActivitiesAsync();
        }
    }
}
