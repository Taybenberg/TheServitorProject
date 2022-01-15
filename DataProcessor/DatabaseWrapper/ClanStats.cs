using BungieNetApi;
using ClanActivitiesDatabase;
using CommonData.DiscordEmoji;
using CommonData.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class ClanStats : IWrapper
    {
        public record Stat
        {
            public string Name { get; internal set; }
            public string Value { get; internal set; }
        }

        public bool IsSuccessful { get; private set; }

        public string Mode { get; private set; }

        public string Emoji { get; private set; }

        public bool IsUserRegistered { get; private set; }

        public IEnumerable<Stat> Stats { get; private set; }

        private readonly string _mode;

        private readonly ulong _userID;

        private readonly IApiClient _apiClient;

        private readonly IClanActivitiesDB _clanDB;

        internal ClanStats(IClanActivitiesDB clanDB, IApiClient apiClient, string mode, ulong discordUserID) =>
            (_clanDB, _apiClient, _mode, _userID) = (clanDB, apiClient, mode, discordUserID);

        public async Task InitAsync()
        {
            var currUser = await _clanDB.GetUserByDiscordIdAsync(_userID);

            IsUserRegistered = currUser is not null;

            if (!IsUserRegistered)
                return;

            var pair = Translation.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == _mode));

            if (!(IsSuccessful = pair.Value is not null))
                return;

            Mode = pair.Value[0];

            Emoji = CommonData.DiscordEmoji.Emoji.GetActivityEmoji(pair.Key);

            var clanStats = await _apiClient.GetClan(currUser.ClanID).GetClanStatsAsync(pair.Key);

            Stats = clanStats.Select(x => new Stat
            {
                Name = Translation.StatNames[x.Stat],
                Value = x.Value
            }).OrderBy(x => x.Name);
        }
    }
}
