using BungieNetApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BungieNetApi.Entities
{
    public class Character
    {
        public long CharacterID { get; internal set; }

        public long MembershipID { get; internal set; }

        public MembershipType MembershipType { get; internal set; }

        private class CharacterContainer
        {
            public DateTime DateLastPlayed;
            public DestinyClass Class;
            public DestinyRace Race;
            public DestinyGender Gender;

            internal CharacterContainer(BungieNetApiClient apiClient, MembershipType membershipType, long membershipID, long characterID)
            {
                var rawCharacter = apiClient.getRawCharacterAsync((int)membershipType, membershipID.ToString(), characterID.ToString()).Result;

                DateLastPlayed = rawCharacter.data.dateLastPlayed;
                Class = (DestinyClass)rawCharacter.data.classType;
                Race = (DestinyRace)rawCharacter.data.raceType;
                Gender = (DestinyGender)rawCharacter.data.genderType;
            }
        }

        private Lazy<CharacterContainer> _container;

        private readonly BungieNetApiClient _apiClient;

        internal Character(BungieNetApiClient apiClient)
        {
            _apiClient = apiClient;

            _container = new(() => new CharacterContainer(_apiClient, MembershipType, MembershipID, CharacterID));
        }

        public DateTime DateLastPlayed
        {
            get
            {
                return _container.Value.DateLastPlayed;
            }
        }

        public DestinyClass Class
        {
            get
            {
                return _container.Value.Class;
            }
        }

        public DestinyRace Race
        {
            get
            {
                return _container.Value.Race;
            }
        }

        public DestinyGender Gender
        {
            get
            {
                return _container.Value.Gender;
            }
        }

        public async Task<IEnumerable<Activity>> GetActivitiesAsync(int count, int page)
        {
            var rawUserActivities = await _apiClient.getRawActivitiesAsync((int)MembershipType, MembershipID.ToString(), CharacterID.ToString(), count, page);

            if (rawUserActivities is null)
                return new List<Activity>();

            return rawUserActivities.Select(x => new Activity(_apiClient)
            {
                InstanceID = long.Parse(x.activityDetails.instanceId),
                Period = x.period
            });
        }
    }
}
