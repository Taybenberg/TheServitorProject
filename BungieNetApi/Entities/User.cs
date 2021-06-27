using BungieNetApi.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BungieNetApi.Entities
{
    public class User
    {
        private readonly BungieNetApiClient _apiClient;
        internal User(BungieNetApiClient apiClient) => _apiClient = apiClient;

        public long MembershipId { get; internal set; }

        public MembershipType MembershipType { get; internal set; }

        public string LastSeenDisplayName { get; internal set; }

        public DateTime ClanJoinDate { get; internal set; }

        private DateTime? _dateLastPlayed = null;
        public DateTime DateLastPlayed
        {
            get
            {
                if (_dateLastPlayed is null)
                    initUserDetailsAsync().Wait();

                return (DateTime)_dateLastPlayed;
            }
        }

        string[] _characterIDs = null;
        public IEnumerable<Character> Characters
        {
            get
            {
                if (_characterIDs is null)
                    initUserDetailsAsync().Wait();

                ConcurrentBag<Character> characters = new();

                Parallel.ForEach(_characterIDs, (chID) =>
                {
                    var rawCharacter = _apiClient.getRawCharacterAsync((int)MembershipType, MembershipId.ToString(), chID).Result;

                    characters.Add(new Character
                    {
                        CharacterId = long.Parse(rawCharacter.data.characterId),
                        MembershipId = long.Parse(rawCharacter.data.membershipId),
                        DateLastPlayed = rawCharacter.data.dateLastPlayed,
                        Class = (DestinyClass)rawCharacter.data.classType,
                        Race = (DestinyRace)rawCharacter.data.raceType,
                        Gender = (DestinyGender)rawCharacter.data.genderType
                    });
                });

                return characters;
            }
        }

        public record ClanInfo
        {
            public string ClanSign { get; internal set; }
            public string ClanName { get; internal set; }
        }

        public async Task<IEnumerable<ClanInfo>> GetUserClansAsync()
        {
            var rawClans = await _apiClient.getRawUserClansAsync((int)MembershipType, MembershipId.ToString());

            return rawClans.Select(x => new ClanInfo
            {
                ClanSign = x.group.clanInfo.clanCallsign,
                ClanName = x.group.name
            });
        }

        public async Task<IEnumerable<Activity>> GetUserActivitiesAsync(long characterId, int count, int page)
        {
            var rawUserActivities = await _apiClient.getRawActivitiesAsync((int)MembershipType, MembershipId.ToString(), characterId.ToString(), count, page);

            if (rawUserActivities is null)
                return new List<Activity>();

            return rawUserActivities.Select(x => new Activity(_apiClient)
            {
                InstanceId = long.Parse(x.activityDetails.instanceId)
            });
        }

        private async Task initUserDetailsAsync()
        {
            var rawProfile = await _apiClient.getRawProfileAsync((int)MembershipType, MembershipId.ToString());

            _dateLastPlayed = rawProfile.data.dateLastPlayed;
            _characterIDs = rawProfile.data.characterIds;
        }
    }
}
