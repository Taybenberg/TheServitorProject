using BungieNetApi.Enums;
using Database;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class MyActivities : IWrapper
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

        public string UserName { get; private set; }

        public int Count { get; private set; }

        public string QuickChartURL { get; private set; }

        public IEnumerable<ClassCounter> Classes { get; private set; }

        public IEnumerable<ModeCounter> Modes { get; private set; }

        private readonly ulong _userID;

        private readonly IClanDB _clanDB;

        private readonly DateTime? _period;

        internal MyActivities(IClanDB clanDB, ulong discordUserID, DateTime? period) => (_clanDB, _userID, _period) = (clanDB, discordUserID, period);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserWithActivitiesAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            UserName = user.UserName;

            Classes = user.Characters.Select(c => new ClassCounter
            {
                Emoji = EmojiContainer.GetClassEmoji(c.Class),
                Class = TranslationDictionaries.ClassNames[c.Class],
                Count = _period is null ? c.ActivityUserStats.Count : 
                c.ActivityUserStats.Where(y => y.Activity.Period > _period).Count()
            }).OrderByDescending(x => x.Count);

            var acts = user.Characters.SelectMany(c => c.ActivityUserStats.Select(z => z.Activity)).Distinct();

            if (_period is not null)
                acts = acts.Where(p => p.Period > _period);

            Count = acts.Count();

            Dictionary<ActivityType, int> counter = new();

            foreach (var at in Enum.GetValues<ActivityType>())
                counter.Add(at, 0);

            foreach (var act in acts)
                counter[act.ActivityType]++;

            var cumulativeCounter = new CumulativeActivityCounter();

            List<ModeCounter> modeCounter = new();

            foreach (var pair in counter)
            {
                if (pair.Value > 0)
                {
                    cumulativeCounter.Add(pair.Key, pair.Value);

                    modeCounter.Add(new ModeCounter
                    {
                        Emoji = EmojiContainer.GetActivityEmoji(pair.Key),
                        Modes = TranslationDictionaries.ActivityNames[pair.Key],
                        Count = pair.Value
                    });
                }
            }

            Modes = modeCounter.OrderByDescending(x => x.Count);

            QuickChartURL = cumulativeCounter.QuickChartURL;
        }
    }
}
