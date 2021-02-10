using System.Threading.Tasks;
using BungieNetApi;
using Database;

namespace ServitorDiscordBot
{
    public static class Extensions
    {
        public static async Task<BungieNetApi.Activity> GetActivityAdditionalDetailsAsync(this Database.Activity activity, BungieNetApiClient apiClient)
        {
            return await apiClient.GetActivityDetailsAsync(activity.ActivityID.ToString());
        }

        public static Database.User GetUser2Async(this Database.UserRelations relation, ClanDatabase database)
        {
            return database.Users.Find(relation.User2ID);
        }
    }
}
