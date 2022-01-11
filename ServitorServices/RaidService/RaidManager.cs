using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RaidDatabase;

namespace RaidService
{
    public class RaidManager : IRaidManager
    {
        private readonly ILogger _logger;
        private readonly IRaidDB _raidDB;

        public RaidManager(ILogger<RaidManager> logger, IRaidDB raidDB) =>
            (_logger, _raidDB) = (logger, raidDB);
    }
}
