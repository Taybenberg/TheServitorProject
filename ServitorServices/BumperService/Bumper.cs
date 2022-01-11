using BumperDatabase;
using Microsoft.Extensions.Logging;

namespace BumperService
{
    public class Bumper : IBumper
    {
        private readonly ILogger _logger;
        private readonly IBumperDB _bumperDB;

        public Bumper(ILogger<Bumper> logger, IBumperDB bumperDB) => 
            (_logger, _bumperDB) = (logger, bumperDB);
    }
}