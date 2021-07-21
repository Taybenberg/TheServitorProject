using Database;
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

        internal MyGrandmasters(IClanDB clanDB, ulong discordUserID, DateTime seasonStart) =>
            (_clanDB, _userID, _seasonStart) = (clanDB, discordUserID, seasonStart);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserByDiscordIdAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            var nightfalls = await _clanDB.GetUserNightfallsAsync(_userID);

            var gms = nightfalls.Where(x => getGMname(x.ReferenceHash) is not null).OrderByDescending(x => x.Period);

            Seasonal = gms.Where(x => x.Period > _seasonStart).Select(x => getGMname(x.ReferenceHash)).Distinct();

            AllTime = gms.Select(x => getGMname(x.ReferenceHash)).Distinct();
        }

        private string getGMname(long hash) =>
            hash switch
            {
                3812135451 => "The Glassway",
                2136458560 => "The Disgraced",
                265186825 => "Broodhold",
                2599001919 => "The Inverted Spire",
                1495545956 => "The Scarlet Keep",
                3233498454 => "Exodus Crash",
                1203950592 => "The Devils' Lair",
                1753547901 => "The Arms Dealer",
                2103025315 => "Proving Grounds",
                3293630132 => "Fallen S.A.B.E.R.",
                3029388704 => "The Insight Terminus",
                557845334 => "Warden of Nothing",
                _ => null
            };
    }
}
