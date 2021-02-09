using System.Collections.Generic;
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
    }
}
