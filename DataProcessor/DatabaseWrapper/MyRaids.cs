using Database;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using DataProcessor.RaidManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataProcessor.DatabaseWrapper
{
    public class MyRaids : IWrapper
    {
        public record RaidContainer
        {
            public string Emoji { get; internal set; }
            public string Name { get; internal set; }
        }

        public record ClassContainer
        {
            public string Emoji { get; internal set; }
            public string Class { get; internal set; }
            public IEnumerable<RaidContainer> Raids { get; internal set; }
        }

        public bool IsUserRegistered { get; private set; }

        public IEnumerable<ClassContainer> Classes { get; private set; }

        public string QuickChartURL { get; private set; }

        private readonly DateTime _seasonStart;

        private readonly int _weekNumber;

        private readonly ulong _userID;

        private readonly IClanDB _clanDB;

        internal MyRaids(IClanDB clanDB, ulong discordUserID, DateTime seasonStart, int weekNumber) =>
            (_clanDB, _userID, _seasonStart, _weekNumber) = (clanDB, discordUserID, seasonStart, weekNumber);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserByDiscordIdAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            var raids = await _clanDB.GetUserRaidsAsync(_userID, _seasonStart.AddDays((_weekNumber - 1) * 7).ToLocalTime());

            int[] classes = new int[3];

            ConcurrentBag<ClassContainer> containers = new();

            Parallel.ForEach(user.Characters, (c) =>
            {
                var charRaids = raids.Where(x => x.ActivityUserStats.Any(y => y.Completed && y.CharacterID == c.CharacterID));

                var types = charRaids.OrderBy(x => x.Period).Select(x => Raid.GetRaidType(x.ReferenceHash)).Distinct();

                classes[(int)c.Class] = types.Count();

                containers.Add(new ClassContainer
                {
                    Emoji = EmojiContainer.GetClassEmoji(c.Class),
                    Class = TranslationDictionaries.ClassNames[c.Class],
                    Raids = types.Select(t => new RaidContainer
                    {
                        Emoji = EmojiContainer.GetRaidEmoji(t),
                        Name = TranslationDictionaries.RaidTypes[t]
                    })
                });
            });

            Classes = containers.OrderByDescending(x => x.Raids.Count());

            var quickChartString = "{type:'polarArea',data:{datasets:[{data:[" +
                string.Join(',', classes) + "],backgroundColor:['#D81B60','#1976D2'," +
                "'#FFC400'],},],labels:['Титан','Мисливець','Варлок'],},options:" +
                "{legend:{labels:{fontColor:'white'}},scale:{display:false}}}";

            QuickChartURL = $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
        }
    }
}
