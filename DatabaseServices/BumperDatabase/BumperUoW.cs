using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumperDatabase
{
    public class BumperUoW : IBumperDB
    {
        private readonly ILogger _logger;
        private readonly BumperContext _context;

        public BumperUoW(ILogger<BumperUoW> logger, BumperContext context) =>
            (_logger, _context) = (logger, context);
    }
}
