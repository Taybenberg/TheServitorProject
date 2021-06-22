using BungieNetApi;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<bool> IsIronBannerAvailableAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

            var ibBillboard = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ib-billboard\"]/div[2]");

            return ibBillboard is not null;
        }

        public static async Task<Activity> GetActivityAdditionalDetailsAsync(this Database.Activity activity, BungieNetApiClient apiClient)
        {
            return await apiClient.GetActivityDetailsAsync(activity.ActivityID.ToString());
        }
    }
}
