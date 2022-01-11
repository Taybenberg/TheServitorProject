using BumperDatabase;
using Microsoft.Extensions.Logging;

namespace BumperService
{
    public class BumpManager : IBumpManager
    {
        private readonly ILogger _logger;
        private readonly IBumperDB _bumperDB;

        public BumpManager(ILogger<BumpManager> logger, IBumperDB bumperDB) => 
            (_logger, _bumperDB) = (logger, bumperDB);
    }
}