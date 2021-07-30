using Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class MyGrandmasters : IWrapper
    {
        public bool IsUserRegistered { get; private set; }

        public IEnumerable<string> Seasonal { get; private set; }

        public IEnumerable<string> AllTime { get; private set; }

        private readonly DateTime _seasonStart;

        private readonly ulong _userID;

        private readonly IClanDB _clanDB;

        private readonly IDictionary<string, string> _GMs;

        internal MyGrandmasters(IClanDB clanDB, ulong discordUserID, DateTime seasonStart, IConfiguration configuration) =>
            (_clanDB, _userID, _seasonStart, _GMs) = (clanDB, discordUserID, seasonStart, configuration.GetSection("Destiny2:Grandmasters").Get<IDictionary<string, string>>());

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserByDiscordIdAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            var nightfalls = await _clanDB.GetUserNightfallsAsync(_userID);

            var gms = nightfalls.Where(x => _GMs.ContainsKey(x.ReferenceHash.ToString())).OrderByDescending(x => x.Period);

            Seasonal = gms.Where(x => x.Period > _seasonStart).Select(x => _GMs[x.ReferenceHash.ToString()]).Distinct();

            AllTime = gms.Select(x => _GMs[x.ReferenceHash.ToString()]).Distinct();
        }
    }
}
