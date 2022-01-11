using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaidDatabase
{
    public class RaidUoW : IRaidDB
    {
        private readonly ILogger _logger;
        private readonly IRaidDB _raidDB;

        public RaidUoW(ILogger<RaidUoW> logger, IRaidDB raidDB) =>
            (_logger, _raidDB) = (logger, raidDB);
    }
}
