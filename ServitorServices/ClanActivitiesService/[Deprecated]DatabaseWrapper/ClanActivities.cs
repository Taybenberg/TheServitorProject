//using BungieNetApi.Enums;
//using ClanActivitiesDatabase;
//using CommonData.DiscordEmoji;
//using CommonData.Localization;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DataProcessor.DatabaseWrapper
//{
//    public class ClanActivities : IWrapper
//    {
//        public record ModeCounter
//        {
//            public string Emoji { get; internal set; }
//            public string[] Modes { get; internal set; }
//            public int Count { get; internal set; }
//        }

//        public int Count { get; private set; }

//        public string QuickChartURL { get; private set; }

//        public IEnumerable<ModeCounter> Modes { get; private set; }

//        private readonly IClanActivitiesDB _clanDB;

//        private readonly DateTime? _period;

//        internal ClanActivities(IClanActivitiesDB clanDB, DateTime? period) => (_clanDB, _period) = (clanDB, period);

//        public async Task InitAsync()
//        {
//            var acts = await _clanDB.GetActivitiesAsync();

//            if (_period is not null)
//                acts = acts.Where(p => p.Period > _period);

//            Count = acts.Count();

//            Dictionary<ActivityType, int> counter = new();

//            foreach (var at in Enum.GetValues<ActivityType>())
//                counter.Add(at, 0);

//            foreach (var act in acts)
//                counter[act.ActivityType]++;

//            var cumulativeCounter = new CumulativeActivityCounter();

//            List<ModeCounter> modeCounter = new();

//            foreach (var pair in counter)
//            {
//                if (pair.Value > 0)
//                {
//                    cumulativeCounter.Add(pair.Key, pair.Value);

//                    modeCounter.Add(new ModeCounter
//                    {
//                        Emoji = Emoji.GetActivityEmoji(pair.Key),
//                        Modes = Translation.ActivityNames[pair.Key],
//                        Count = pair.Value
//                    });
//                }
//            }

//            Modes = modeCounter.OrderByDescending(x => x.Count);

//            QuickChartURL = cumulativeCounter.QuickChartURL;
//        }
//    }
//}
