using BungieNetApi.Enums;
using Database;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class ClanActivities : IWrapper
    {
        public record ModeCounter
        {
            public string Emoji { get; internal set; }
            public string[] Modes { get; internal set; }
            public int Count { get; internal set; }
        }

        public int Count { get; private set; }

        public string QuickChartURL { get; private set; }

        public IEnumerable<ModeCounter> Modes { get; private set; }

        private readonly IClanDB _clanDB;

        internal ClanActivities(IClanDB clanDB) => _clanDB = clanDB;

        public async Task InitAsync()
        {
            var acts = await _clanDB.GetActivitiesAsync();

            Count = acts.Count();

            var cumulativeCounter = new CumulativeActivityCounter();

            ConcurrentBag<ModeCounter> counter = new();

            Parallel.ForEach((ActivityType[])Enum.GetValues(typeof(ActivityType)), (type) =>
            {
                var count = acts.Count(x => x.ActivityType == type);

                if (count > 0)
                {
                    cumulativeCounter.Add(type, count);

                    counter.Add(new ModeCounter
                    {
                        Emoji = EmojiContainer.GetActivityEmoji(type),
                        Modes = TranslationDictionaries.ActivityNames[type],
                        Count = count
                    });
                }
            });

            Modes = counter.OrderByDescending(x => x.Count);

            QuickChartURL = cumulativeCounter.QuickChartURL;
        }
    }
}
