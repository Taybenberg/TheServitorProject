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

        public string BungieName { get; internal set; }

        public DateTime ClanJoinDate { get; internal set; }

        public long ClanID { get; internal set; }

        private class UserContainer
        {
            public DateTime DateLastPlayed;
            public IEnumerable<Character> Characters;

            internal UserContainer(BungieNetApiClient apiClient, MembershipType membershipType, long membershipID)
            {
                var rawProfile = apiClient.getRawProfileWithCharactersAsync((int)membershipType, membershipID.ToString()).Result;

                DateLastPlayed = rawProfile.profile.data.dateLastPlayed;

                Characters = rawProfile.characters.data.Values.Select(x =>
                new Character(apiClient)
                {
                    CharacterID = long.Parse(x.characterId),
                    MembershipID = membershipID,
                    MembershipType = membershipType,
                    DateLastPlayed = x.dateLastPlayed,
                    Class = (DestinyClass)x.classType,
                    Race = (DestinyRace)x.raceType,
                    Gender = (DestinyGender)x.genderType
                });
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
                return _container.Value.Characters;
            }
        }

        public record ClanInfo
        {
            public string ClanSign { get; internal set; }
            public string ClanName { get; internal set; }
        }

        public async Task<ClanInfo> GetUserClanAsync()
        {
            var rawClans = await _apiClient.getRawUserClansAsync((int)MembershipType, MembershipID.ToString());

            return rawClans.Select(x => new ClanInfo
            {
                ClanSign = x.group.clanInfo.clanCallsign,
                ClanName = x.group.name
            }).FirstOrDefault();
        }
    }
}
