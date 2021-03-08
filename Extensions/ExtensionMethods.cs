using BungieNetApi;
using Flurl.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<Activity> GetActivityAdditionalDetailsAsync(this Database.Activity activity, BungieNetApiClient apiClient)
        {
            return await apiClient.GetActivityDetailsAsync(activity.ActivityID.ToString());
        }

        public static async Task<Stream> GetXurInventoryAsync(this BungieNetApiClient apiClient)
        {
            var items = await apiClient.GetXurItemsAsync();

            using var background = new MemoryStream(ExtensionsRes.XurItemsBackground);

            Image image = new Bitmap(background);

            using (var g = Graphics.FromImage(image))
            {
                int x = 0;
                int y = 0;

                Brush brush = new SolidBrush(Color.Black);

                Font itemName = new Font("Times New Roman", 20);
                Font itemType = new Font("Times New Roman", 14);

                foreach (var item in items.Reverse())
                {
                    using var stream = await item.ItemIconUrl.GetStreamAsync();

                    g.DrawImageUnscaled(new Bitmap(stream), x, y);

                    g.DrawString(item.ItemName, itemName, brush, x + 110, y + 30);
                    g.DrawString(item.ItemTypeAndTier, itemType, brush, x + 110, y + 60);

                    y += 96;
                }
            }

            var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return ms;
        }
    }
}
