﻿using BungieNetApi;
using Extensions.Inventory;
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
    public class XurParser : IInventoryParser<XurInventory>
    {
        private BungieNetApiClient _apiClient;
        private bool _getLocation;

        public XurParser(BungieNetApiClient apiClient, bool getLocation = false) =>
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

            foreach (var item in items.Reverse())
            {
                inventory.XurItems.Add(new XurItem
                {
                    ItemName = item.ItemName,
                    ItemClass = Localization.ItemNames[item.ItemTypeAndTier],
                    ItemIconURL = item.ItemIconUrl
                });
            }

            return inventory;
        }

        public async Task<Stream> GetImageAsync()
        {
            using Image image = Image.Load(ExtensionsRes.XurItemsBackground);

            var inventory = await GetInventoryAsync();

            Font locationFont = new Font(SystemFonts.Find("Arial"), 28, FontStyle.Bold);
            image.Mutate(m => m.DrawText(inventory.Location, locationFont, Color.Black, new Point(257, 574)));

            Font itemName = new Font(SystemFonts.Find("Arial"), 34);
            Font itemType = new Font(SystemFonts.Find("Arial"), 23);

            int Yi = 30, Yt1 = 43, Yt2 = 89;
            int interval = 136;

            foreach (var item in inventory.XurItems)
            {
                using var stream = await item.ItemIconURL.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);

                image.Mutate(m =>
                {
                    m.DrawImage(icon, new Point(30, Yi), 1);

                    m.DrawText(item.ItemName, itemName, Color.Black, new Point(146, Yt1));
                    m.DrawText(item.ItemClass, itemType, Color.Black, new Point(153, Yt2));
                });

                Yi += interval;
                Yt1 += interval;
                Yt2 += interval;
            }
            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}