using BungieNetApi;
using Database;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataProcessor.DatabaseStats
{
    public class Leaderboard : IStats
    {
        public record Entry
        {
            public bool IsCurrUser { get; internal set; }
            public int Rank { get; internal set; }
            public string UserName { get; internal set; }
            public string Class { get; internal set; }
            public string Value { get; internal set; }
        }

        public record Stat
        {
            public string Name { get; internal set; }
            public IEnumerable<Entry> Entries { get; internal set; }
        }

        public IEnumerable<Stat> Stats { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string Mode { get; private set; }

        public string Emoji { get; private set; }

        public bool UserRegistered { get; private set; }

        public string QuickChartURL { get; private set; }

        private string _mode;

        private ulong _userID;

        private readonly IApiClient _apiClient;

        private readonly IClanDB _clanDB;

        internal Leaderboard(IClanDB clanDB, IApiClient apiClient, string mode, ulong discordUserID) =>
            (_clanDB, _apiClient, _mode, _userID) = (clanDB, apiClient, mode, discordUserID);

        public async Task InitAsync()
        {
            var pair = TranslationDictionaries.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == _mode));

            if (!(IsSuccessful = pair.Value is not null))
                return;

            Mode = pair.Value[0];

            Emoji = EmojiContainer.GetActivityEmoji(pair.Key);

            var leaderboard = await _apiClient.Clan.GetClanLeaderboardAsync(pair.Key, TranslationDictionaries.StatNames.Keys.ToArray());

            if (!leaderboard.Any())
            {
                Stats = new List<Stat>();

                return;
            }

            var currUser = await _clanDB.GetUserByDiscordIdAsync(_userID);

            UserRegistered = currUser is not null;

            var users = await _clanDB.GetUsersAsync();

            ConcurrentBag<Stat> stats = new();

            Parallel.ForEach(leaderboard, (entry) =>
            {
                if (entry.Leaders.Count() > 0)
                {
                    List<Entry> entries = new();

                    bool userFound = false;

                    foreach (var user in entry.Leaders.Take(3))
                    {
                        var u = users.FirstOrDefault(x => x.UserID == user.UserID);

                        if (u is null)
                            continue;

                        if (u.UserID == currUser?.UserID)
                            userFound = true;

                        entries.Add(new Entry
                        {
                            IsCurrUser = u.UserID == currUser?.UserID,
                            Rank = user.Rank,
                            UserName = u.UserName,
                            Class = TranslationDictionaries.ClassNames[user.Class],
                            Value = user.Value
                        });
                    }

                    if (!userFound && UserRegistered)
                    {
                        var u = entry.Leaders.FirstOrDefault(x => x.UserID == currUser.UserID);

                        if (u is not null)
                            entries.Add(new Entry
                            {
                                IsCurrUser = true,
                                Rank = u.Rank,
                                UserName = currUser.UserName,
                                Class = TranslationDictionaries.ClassNames[u.Class],
                                Value = u.Value
                            });
                    }

                    stats.Add(new Stat
                    {
                        Name = TranslationDictionaries.StatNames[entry.Stat],
                        Entries = entries
                    });
                }
            });

            Stats = stats.OrderBy(x => x.Name);

            if (UserRegistered)
            {
                var quickChartString = "{type:'radar',data:{labels:[" + string.Join(',', Stats.Select(x => $"'{x.Name}'")) +
                    "],datasets:[{borderColor:'#25C486',backgroundColor:'rgba(37,196,134,0.5)',pointBackgroundColor:'#25C486'," +
                    "data:[" + string.Join(',', Stats.Select(x => 100 - x.Entries.First(y => y.IsCurrUser).Rank)) + "]}],}," +
                    "options:{legend:{display:false},scale:{angleLines:{color:'rgba(255,255,255,0.5)'},ticks:{display:false," +
                    "suggestedMin:0,suggestedMax:99},gridLines:{color:'rgba(255,255,255,0.5)'},pointLabels:{fontColor:'white'}}}}";

                QuickChartURL = $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
            }
        }
    }
}
