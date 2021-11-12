using BungieNetApi;
using DataProcessor.Parsers.Inventory;
using HtmlAgilityPack;
using NetVips;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataProcessor.Parsers
{
    public class XurParser : IInventoryParser<XurInventory>
    {
        private readonly IApiClient _apiClient;

        private readonly bool _getLocation;

        public XurParser(IApiClient apiClient, bool getLocation) =>
            (_apiClient, _getLocation) = (apiClient, getLocation);

        public async Task<XurInventory> GetInventoryAsync()
        {
            var inventory = new XurInventory();

            var items = await _apiClient.GetXurItemsAsync();

            string location = string.Empty;

            if (_getLocation)
            {
                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://xur.wiki/");
                location = HttpUtility.HtmlEncode(htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[1]/div/div/h1")?.InnerText.Trim() ?? string.Empty);
            }

            if (string.IsNullOrWhiteSpace(location))
                location = "Невизначено";

            inventory.Location = location;

            try
            {
                foreach (var item in items.Reverse())
                {
                    inventory.XurItems.Add(new XurItem
                    {
                        ItemName = item.ItemName,
                        ItemClass = Localization.TranslationDictionaries.ItemNames[item.ItemTypeAndTier],
                        ItemIconURL = item.ItemIconUrl
                    });
                }
            }
            catch { }

            return inventory;
        }

        public async Task<Stream> GetImageAsync()
        {
            var inventory = await GetInventoryAsync();

            var loader = new ImageLoader();

            Image image = Image.NewFromBuffer(ExtensionsRes.XurItemsBackground);

            using var lightLevel = ImageLoader
                .RenderText($"<b>{inventory.Location}</b>", "Arial 28", new int[] { 0, 0, 0 });
            image = image.Composite(lightLevel, Enums.BlendMode.Over, 259, 580);

            int Yi = 30, Yt1 = 49, Yt2 = 95;
            int interval = 136;

            foreach (var item in inventory.XurItems)
            {
                using var icon = await loader.GetImageAsync(item.ItemIconURL);
                image = image.Composite(icon, Enums.BlendMode.Over, 30, Yi);

                using var itemName = ImageLoader
                .RenderText(item.ItemName, "Arial 34", new int[] { 0, 0, 0 });
                image = image.Composite(itemName, Enums.BlendMode.Over, 148, Yt1);

                using var itemClass = ImageLoader
                .RenderText(item.ItemClass, "Arial 23", new int[] { 0, 0, 0 });
                image = image.Composite(itemClass, Enums.BlendMode.Over, 155, Yt2);

                Yi += interval;
                Yt1 += interval;
                Yt2 += interval;
            }

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
