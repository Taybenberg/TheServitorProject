using System.Threading.Tasks;
using BungieNetApi;
using Database;

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<BungieNetApi.Activity> GetActivityAdditionalDetailsAsync(this Database.Activity activity, BungieNetApiClient apiClient)
        {
            return await apiClient.GetActivityDetailsAsync(activity.ActivityID.ToString());
        }
    }
}
