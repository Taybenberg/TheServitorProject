using BungieNetApi.Entities;
using BungieNetApi.Enums;

namespace BungieNetApi
{
    public class EntityFactory : IEntityFactory
    {
        private readonly BungieNetApiClient _apiClient;

        internal EntityFactory(BungieNetApiClient apiClient) => _apiClient = apiClient;

        public User GetUser(long membershipID, MembershipType membershipType) =>
            new User(_apiClient)
            {
                MembershipID = membershipID,
                MembershipType = membershipType
            };

        public Character GetCharacter(long characterID, long membershipID, MembershipType membershipType) =>
            new Character(_apiClient)
            {
                CharacterID = characterID,
                MembershipID = membershipID,
                MembershipType = membershipType
            };

        public Activity GetActivity(long instanceID) =>
            new Activity(_apiClient)
            {
                InstanceID = instanceID
            };
    }
}
