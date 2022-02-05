using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DestinyNotificationsService
{
    public partial class DestinyNotificationsManager : IDestinyNotifications
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DestinyNotificationsManager(ILogger<DestinyNotificationsManager> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);
        }
    }
}