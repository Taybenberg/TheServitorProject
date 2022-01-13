using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ActivityService
{
    public class ActivityManager : IActivityManager
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly BackgroundJobServer _server;

        public ActivityManager(ILogger<ActivityManager> logger, IServiceScopeFactory scopeFactory)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            GlobalConfiguration.Configuration.UseMemoryStorage();

            _server = new();
        }

        public void Init()
        {

        }
    }
}
