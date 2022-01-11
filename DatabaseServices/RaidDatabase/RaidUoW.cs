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
        private readonly RaidContext _context;

        public RaidUoW(ILogger<RaidUoW> logger, RaidContext context) =>
            (_logger, _context) = (logger, context);
    }
}
