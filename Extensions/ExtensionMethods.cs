using BungieNetApi;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<BungieNetApi.Activity> GetActivityAdditionalDetailsAsync(this Database.Activity activity, BungieNetApiClient apiClient)
        {
            return await apiClient.GetActivityDetailsAsync(activity.ActivityID.ToString());
        }

        public static async Task<Stream> GetXurInventoryAsync(this BungieNetApiClient apiClient)
        {
            return new MemoryStream(ExtensionsRes.XurItemsBackground);
        }
    }
}
