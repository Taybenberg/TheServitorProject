﻿using BungieNetApi;
using Database;
using DataProcessor.Localization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseStats
{
    public class MyPartners : IStats
    {
        private ulong _userID;

        private readonly IClanDB _clanDB;

        internal MyPartners(IClanDB clanDB, ulong discordUserID) => (_clanDB, _userID) = (clanDB, discordUserID);

        public async Task InitAsync()
        {

        }
    }
}
