using Microsoft.Extensions.Logging;

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
