using Database;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseStats
{
    public class MyPartners : IStats
    {
        public record PartnerCounter
        {
            public string UserName { get; internal set; }
            public int Count { get; internal set; }
        }

        public bool IsUserRegistered { get; private set; }

        public IEnumerable<PartnerCounter> Partners { get; private set; }

        private ulong _userID;

        private readonly IClanDB _clanDB;

        internal MyPartners(IClanDB clanDB, ulong discordUserID) => (_clanDB, _userID) = (clanDB, discordUserID);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserWithActivitiesAndOtherUserStatsAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            var acts = user.Characters.SelectMany(x => x.ActivityUserStats.Select(y => y.Activity)).Distinct();

            var users = (await _clanDB.GetUsersWithCharactersAsync()).Where(x => x.UserID != user.UserID);

            ConcurrentBag<PartnerCounter> counter = new();

            Parallel.ForEach(users, (usr) =>
            {
                var count = acts.Where(x => x.ActivityUserStats.Any(y => usr.Characters.Any(c => c.CharacterID == y.CharacterID))).Count();

                if (count > 0)
                    counter.Add(new PartnerCounter
                    {
                        UserName = usr.UserName,
                        Count = count
                    });
            });

            Partners = counter.OrderByDescending(x => x.Count);
        }
    }
}
