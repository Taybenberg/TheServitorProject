using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClanActivitiesService
{
    public class ClanActivitiesManager : IClanActivities
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ClanActivitiesManager(ILogger<ClanActivitiesManager> logger, IServiceScopeFactory scopeFactory) =>
            (_logger, _scopeFactory) = (logger, scopeFactory);
    }
}
