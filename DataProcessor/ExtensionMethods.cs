using BungieNetApi;
using BungieNetApi.Entities;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace DataProcessor
{
    public static class ExtensionMethods
    {
        public static async Task<bool> IsIronBannerAvailableAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

            var ibBillboard = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ib-billboard\"]/div[2]");

            return ibBillboard is not null;
        }

        public static Activity GetDestinyActivity(this Database.Activity activity, IApiClient apiClient)
        {
            return apiClient.EntityFactory.GetActivity(activity.ActivityID);
        }

        public static User GetDestinyUser(this Database.User user, IApiClient apiClient)
        {
            return apiClient.EntityFactory.GetUser(user.UserID, user.MembershipType);
        }

        public static Character GetDestinyCharacter(this Database.Character character, IApiClient apiClient)
        {
            return apiClient.EntityFactory.GetCharacter(character.CharacterID, character.UserID, character.User.MembershipType);
        }
    }
}
