using BungieNetApi.Enums;
using Database;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataProcessor.DatabaseStats
{
    public class MyActivities : IStats
    {
        public record ClassCounter
        {
            public string Emoji { get; internal set; }
            public string Class { get; internal set; }
            public int Count { get; internal set; }
        }

        public record ModeCounter
        {
            public string Emoji { get; internal set; }
            public string[] Modes { get; internal set; }
            public int Count { get; internal set; }
        }

        public bool IsUserRegistered { get; private set; }

        public int Count { get; private set; }

        public string QuickChartURL { get; private set; }

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
                Emoji = EmojiContainer.GetClassEmoji(c.Class),
                Class = TranslationDictionaries.ClassNames[c.Class],
                Count = c.ActivityUserStats.Count
            });

            var acts = user.Characters.SelectMany(c => c.ActivityUserStats.Select(z => z.Activity)).Distinct();

            Count = acts.Count();

            var cumulativeCounter = new CumulativeActivityCounter();

            ConcurrentBag<ModeCounter> modeCounter = new();

            Parallel.ForEach((ActivityType[])Enum.GetValues(typeof(ActivityType)), (type) =>
            {
                var count = acts.Count(x => x.ActivityType == type);

                if (count > 0)
                {
                    cumulativeCounter.Add(type, count);

                    modeCounter.Add(new ModeCounter
                    {
                        Emoji = EmojiContainer.GetActivityEmoji(type),
                        Modes = TranslationDictionaries.ActivityNames[type],
                        Count = count
                    });
                }
            });

            Modes = modeCounter.OrderByDescending(x => x.Count);

            QuickChartURL = cumulativeCounter.QuickChartURL;
        }
    }
}
