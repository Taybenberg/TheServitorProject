using BungieNetApi.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BungieNetApi.Entities
{
    public class Clan
    {
        public long ID { get; init; }

        private readonly BungieNetApiClient _apiClient;
        internal Clan(BungieNetApiClient apiClient, long clanID) => (_apiClient, ID) = (apiClient, clanID);

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            ConcurrentBag<User> users = new();

            var rawClanMembers = await _apiClient.getRawUsersAsync(ID.ToString());

            return rawClanMembers.Select(x => new User(_apiClient)
            {
                MembershipID = long.Parse(x.destinyUserInfo.membershipId),
                MembershipType = (MembershipType)x.destinyUserInfo.membershipType,
                LastSeenDisplayName = x.destinyUserInfo.LastSeenDisplayName,
                ClanJoinDate = x.joinDate
            });
        }

        public record ClanStat
        {
            public string Stat { get; internal set; }
            public string Value { get; internal set; }
        }

        public async Task<IEnumerable<ClanStat>> GetClanStatsAsync(ActivityType activityType)
        {
            var rawStats = await _apiClient.getRawClanStatsAsync(ID.ToString(), (int)activityType);

            return rawStats.Select(x => new ClanStat
            {
                Stat = x.statId,
                Value = x.value.basic.displayValue
            });
        }

        public record StatLeaders
        {
            public int Rank { get; internal set; }
            public long UserID { get; internal set; }
            public DestinyClass Class { get; internal set; }
            public string Value { get; set; }
        }

        public record Leaderboard
        {
            public string Stat { get; internal set; }
            public IEnumerable<StatLeaders> Leaders { get; internal set; }
        }

        public async Task<IEnumerable<Leaderboard>> GetClanLeaderboardAsync(ActivityType activityType, string[] modes)
        {
            ConcurrentBag<Leaderboard> leaderboard = new();

            Parallel.ForEach(modes, (mode) =>
            {
                var rawLeaderboard = _apiClient.getRawClanLeaderboardAsync(ID.ToString(), (int)activityType, 100, mode).Result;

                if (rawLeaderboard is not null && rawLeaderboard.Response.Any())
                {
                    var value = rawLeaderboard.Response.FirstOrDefault().Value;

                    if (value.ContainsKey(mode))
                    {
                        leaderboard.Add(new Leaderboard
                        {
                            Stat = mode,
                            Leaders = value[mode].entries.Select(x => new StatLeaders
                            {
                                Rank = x.rank,
                                UserID = long.Parse(x.player.destinyUserInfo.membershipId),
                                Class = Enum.Parse<DestinyClass>(x.player.characterClass ?? "Unknown"),
                                Value = x.value.basic.displayValue
                            })
                        });
                    }
                }
            });

            return leaderboard;
        }
    }
}
