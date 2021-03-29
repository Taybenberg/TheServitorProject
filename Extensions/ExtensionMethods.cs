using BungieNetApi;
using Flurl.Http;
using HtmlAgilityPack;
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

        public static async Task<Stream> GetXurInventoryAsync(this BungieNetApiClient apiClient, bool getLocation = false)
        {
            var items = await apiClient.GetXurItemsAsync();

            using var background = new MemoryStream(ExtensionsRes.XurItemsBackground);

            using Image image = Image.FromStream(background);

            using (var g = Graphics.FromImage(image))
            {
                int Xi = 30, Yi = 30;
                int Xt1 = 145, Yt1 = 43;
                int Xt2 = 152, Yt2 = 87;

                int interval = 136;

                Brush brush = new SolidBrush(Color.Black);

                Font itemName = new Font("Arial", 25);
                Font itemType = new Font("Arial", 16);

                foreach (var item in items.Reverse())
                {
                    using var stream = await item.ItemIconUrl.GetStreamAsync();
                    using Image icon = Image.FromStream(stream);

                    g.DrawImage(icon, Xi, Yi);

                    g.DrawString(item.ItemName, itemName, brush, Xt1, Yt1);
                    g.DrawString(item.ItemTypeAndTier, itemType, brush, Xt2, Yt2);

                    Yi += interval;
                    Yt1 += interval;
                    Yt2 += interval;
                }

                int Xt = 254, Yt = 574;

                Font locationFont = new Font("Arial", 20, FontStyle.Bold);

                HtmlNode location = null;

                if (getLocation)
                {
                    var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://xur.wiki/");
                    location = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[1]/div/div/h1");
                }

                var locationName = location is null ? "Невизначено" : location.InnerText.Trim();

                g.DrawString(locationName, locationFont, brush, Xt, Yt);
            }

            var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return ms;
        }
    }
}
