using BungieNetApi.Enums;
using ClanActivitiesDatabase;
using F23.StringSimilarity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class FindUserByName : IWrapper
    {
        public record UserSimilarity
        {
            public string UserName { get; internal set; }
            public long UserId { get; internal set; }
            public MembershipType MembershipType { get; internal set; }
            public double Similarity { get; internal set; }
        }

        public bool UserRegistered { get; private set; }

        public IEnumerable<UserSimilarity> UserSimilarities { get; private set; }

        private readonly IClanActivitiesDB _clanDB;

        private readonly string _discordUserName;

        private readonly ulong _discordUserID;

        internal FindUserByName(IClanActivitiesDB clanDB, ulong discordUserID, string discordUserName) => (_clanDB, _discordUserID, _discordUserName) = (clanDB, discordUserID, discordUserName);

        public async Task InitAsync()
        {
            if (UserRegistered = _clanDB.IsDiscordUserRegistered(_discordUserID))
                return;

            var users = await _clanDB.GetUsersAsync();

            var jw = new JaroWinkler();

            UserSimilarities = users.Where(x => x.DiscordUserID is null)
                .Select(x => new UserSimilarity
                {
                    UserName = x.UserName,
                    UserId = x.UserID,
                    MembershipType = x.MembershipType,
                    Similarity = jw.Similarity(_discordUserName, x.UserName.ToLower())
                })
                .Where(x => x.Similarity > 0.9)
                .OrderByDescending(x => x.Similarity)
                .Take(10);
        }
    }
}
