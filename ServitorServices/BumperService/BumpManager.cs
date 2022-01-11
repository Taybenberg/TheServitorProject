using BumperDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BumperService
{
    public class BumpManager : IBumpManager
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public BumpManager(ILogger<BumpManager> logger, IServiceScopeFactory scopeFactory) => 
            (_logger, _scopeFactory) = (logger, scopeFactory);
    }
}