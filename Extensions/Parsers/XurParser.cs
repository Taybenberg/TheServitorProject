using BungieNetApi;
using Flurl.Http;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Extensions.Parsers
{
    public class XurParser : IInventoryParser
    {
        private BungieNetApiClient _apiClient;
        private bool _getLocation;

        public XurParser(BungieNetApiClient apiClient, bool getLocation = false) =>
            (_apiClient, _getLocation) = (apiClient, getLocation);

        public async Task<Stream> GetImageAsync()
        {
            var items = await _apiClient.GetXurItemsAsync();

            using Image image = Image.Load(ExtensionsRes.XurItemsBackground);

            int Xi = 30, Yi = 30;
            int Xt1 = 146, Yt1 = 43;
            int Xt2 = 153, Yt2 = 89;

            int interval = 136;

            Font itemName = new Font(SystemFonts.Find("Arial"), 34);
            Font itemType = new Font(SystemFonts.Find("Arial"), 23);

            foreach (var item in items.Reverse())
            {
                using var stream = await item.ItemIconUrl.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);
                image.Mutate(m => m.DrawImage(icon, new Point(Xi, Yi), 1));

                image.Mutate(m => m.DrawText(item.ItemName, itemName, Color.Black, new Point(Xt1, Yt1)));
                image.Mutate(m => m.DrawText(Localization.ItemNames[item.ItemTypeAndTier], itemType, Color.Black, new Point(Xt2, Yt2)));

                Yi += interval;
                Yt1 += interval;
                Yt2 += interval;
            }

            int Xt = 257, Yt = 574;

            Font locationFont = new Font(SystemFonts.Find("Arial"), 28, FontStyle.Bold);

            HtmlNode location = null;

            if (_getLocation)
            {
                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://xur.wiki/");
                location = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[1]/div/div/h1");
            }

            var locationName = location?.InnerText.Trim() ?? "Невизначено";
            image.Mutate(m => m.DrawText(HttpUtility.HtmlEncode(locationName), locationFont, Color.Black, new Point(Xt, Yt)));

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
