using BungieNetApi.Entities;
using BungieNetApi.Enums;

namespace BungieNetApi
{
    public interface IEntityFactory
    {
        User GetUser(long membershipID, MembershipType membershipType);

        Character GetCharacter(long characterID, long membershipID, MembershipType membershipType);

        Activity GetActivity(long instanceID);
    }
}
