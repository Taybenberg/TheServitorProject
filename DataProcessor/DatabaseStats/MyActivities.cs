using BungieNetApi;
using Database;
using DataProcessor.Localization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BungieNetApi.Enums;
using Database;
using DataProcessor.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseStats
{
    public class MyActivities : IStats
    {
        public record ClassCounter
        {
            public string Class { get; internal set; }
            public int Count { get; internal set; }
        }

        public record ModeCounter
        {
            public string[] Modes { get; internal set; }
            public int Count { get; internal set; }
        }

        public bool IsUserRegistered { get; private set; }

        public int Count { get; private set; }

        public IEnumerable<ClassCounter> Classes { get; private set; }

        public IEnumerable<ModeCounter> Modes { get; private set; }

        private ulong _userID;

        private readonly IClanDB _clanDB;

        internal MyActivities(IClanDB clanDB, ulong discordUserID) => (_clanDB, _userID) = (clanDB, discordUserID);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserWithActivitiesAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            Classes = user.Characters.Select(c => new ClassCounter
            {
                Class = TranslationDictionaries.ClassNames[c.Class],
                Count = c.ActivityUserStats.Count
            }).OrderByDescending(x => x.Count);

            var acts = user.Characters.SelectMany(c => c.ActivityUserStats.Select(z => z.Activity)).Distinct();

            Count = acts.Count();

            ConcurrentBag<ModeCounter> modeCounter = new();

            Parallel.ForEach((ActivityType[])Enum.GetValues(typeof(ActivityType)), (type) =>
            {
                var count = acts.Count(x => x.ActivityType == type);

                if (count > 0)
                {
                    modeCounter.Add(new ModeCounter
                    {
                        Modes = TranslationDictionaries.ActivityNames[type],
                        Count = count
                    });
                }
            });

            Modes = modeCounter.OrderByDescending(x => x.Count);
        }
    }
}
