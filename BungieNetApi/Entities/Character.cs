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

        public DateTime DateLastPlayed { get; internal set; }

        public DestinyClass Class { get; internal set; }

        public DestinyRace Race { get; internal set; }

        public DestinyGender Gender { get; internal set; }

        private readonly BungieNetApiClient _apiClient;

        internal Character(BungieNetApiClient apiClient) => _apiClient = apiClient;

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
