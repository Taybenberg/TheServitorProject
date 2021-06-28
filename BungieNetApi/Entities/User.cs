using BungieNetApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BungieNetApi.Entities
{
    public class User
    {
        public long MembershipID { get; internal set; }

        public MembershipType MembershipType { get; internal set; }

        public string LastSeenDisplayName { get; internal set; }

        public DateTime ClanJoinDate { get; internal set; }

        private class UserContainer
        {
            public DateTime DateLastPlayed;

            public string[] CharacterIDs;

            internal UserContainer(BungieNetApiClient apiClient, MembershipType membershipType, long membershipID)
            {
                var rawProfile = apiClient.getRawProfileAsync((int)membershipType, membershipID.ToString()).Result;

                DateLastPlayed = rawProfile.data.dateLastPlayed;
                CharacterIDs = rawProfile.data.characterIds;
            }
        }

        private Lazy<UserContainer> _container;

        private readonly BungieNetApiClient _apiClient;

        internal User(BungieNetApiClient apiClient)
        {
            _apiClient = apiClient;

            _container = new(() => new UserContainer(_apiClient, MembershipType, MembershipID));
        }

        public DateTime DateLastPlayed
        {
            get
            {
                return _container.Value.DateLastPlayed;
            }
        }

        public IEnumerable<Character> Characters
        {
            get
            {
                return _container.Value.CharacterIDs.Select(x => new Character(_apiClient)
                {
                    CharacterID = long.Parse(x),
                    MembershipID = MembershipID,
                    MembershipType = MembershipType
                });
            }
        }

        public record ClanInfo
        {
            public string ClanSign { get; internal set; }
            public string ClanName { get; internal set; }
        }

        public async Task<IEnumerable<ClanInfo>> GetUserClansAsync()
        {
            var rawClans = await _apiClient.getRawUserClansAsync((int)MembershipType, MembershipID.ToString());

            return rawClans.Select(x => new ClanInfo
            {
                ClanSign = x.group.clanInfo.clanCallsign,
                ClanName = x.group.name
            });
        }
    }
}
