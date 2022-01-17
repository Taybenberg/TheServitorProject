using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DestinyNotificationsService
{
    public class DestinyNotificationsManager : IDestinyNotifications
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DestinyNotificationsManager(ILogger<DestinyNotificationsManager> logger, IServiceScopeFactory scopeFactory) =>
            (_logger, _scopeFactory) = (logger, scopeFactory);
    }
}