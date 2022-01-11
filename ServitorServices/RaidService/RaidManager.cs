using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaidDatabase;

namespace RaidService
{
    public class RaidManager : IRaidManager
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RaidManager(ILogger<RaidManager> logger, IServiceScopeFactory scopeFactory) =>
            (_logger, _scopeFactory) = (logger, scopeFactory);
    }
}
