using BungieNetApi;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseStats
{
    public class ClanStats : IStats
    {
        public record Stat
        {
            public string Name { get; internal set; }
            public string Value { get; internal set; }
        }

        public bool IsSuccessful { get; private set; }

        public string Mode { get; private set; }

        public string Emoji { get; private set; }

        public IEnumerable<Stat> Stats { get; private set; }

        private string _mode;

        private readonly IApiClient _apiClient;

        internal ClanStats(IApiClient apiClient, string mode) => (_apiClient, _mode) = (apiClient, mode);

        public async Task InitAsync()
        {
            var pair = TranslationDictionaries.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == _mode));

            if (!(IsSuccessful = pair.Value is not null))
                return;

            Mode = pair.Value[0];

            Emoji = EmojiContainer.GetActivityEmoji(pair.Key);

            var clanStats = await _apiClient.Clan.GetClanStatsAsync(pair.Key);

            Stats = clanStats.Select(x => new Stat
            {
                Name = TranslationDictionaries.StatNames[x.Stat],
                Value = x.Value
            });
        }
    }
}
