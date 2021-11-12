using DataProcessor.Parsers.Inventory;
using HtmlAgilityPack;
using NetVips;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor.Parsers
{
    public class ResourcesParser : IInventoryParser<ResourcesInventory>
    {
        public async Task<ResourcesInventory> GetInventoryAsync()
        {
            var inventory = new ResourcesInventory();

            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17).ToLocalTime();

            if (currDate.Hour < 17)
            {
                inventory.ResetBegin = resetTime.AddDays(-1);
                inventory.ResetEnd = resetTime;
            }
            else
            {
                inventory.ResetBegin = resetTime;
                inventory.ResetEnd = resetTime.AddDays(1);
            }

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/vendors");

            int[][] vendorsInt = { new int[] { 3, 1, 1 }, new int[] { 7, 2, 2 } };
            string[] vendors = { "//*[@id=\"vendors_863940356_content\"]/div[5]/div", "//*[@id=\"vendors_672118013_content\"]/div[4]/div", "//*[@id=\"vendors_350061650_content\"]/div[4]/div" };

            int i = 0;

            foreach (var vendor in vendors)
            {
                var container = htmlDoc.DocumentNode.SelectSingleNode(vendor);

                if (container is not null)
                {
                    for (int j = vendorsInt[0][i]; j <= vendorsInt[1][i]; j++)
                    {
                        var item = new ResourceItem
                        {
                            ResourceName = container.SelectSingleNode($"./div[{j}]/div[3]/div[1]/p")
                            .InnerText.Replace("Purchase ", ""),

                            ResourceIconURL = (container.SelectSingleNode($"./div[{j}]/div[1]/img[2]") ??
                            container.SelectSingleNode($"./div[{j}]/div[1]/img"))
                            .Attributes["src"].Value,

                            ResourceCurrencyQuantity =
                            (container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[3]/div/p[2]") ??
                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[4]/div/p[2]") ??
                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[5]/div/p[2]"))
                            .InnerText,

                            ResourceCurrencyIconURL =
                            (container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[3]/div/div/img") ??
                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[4]/div/div/img") ??
                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[5]/div/div/img"))
                            .Attributes["src"].Value
                        };

                        switch (i)
                        {
                            case 0:
                                inventory.SpiderResources.Add(item);
                                break;

                            case 1:
                                inventory.Banshee44Resources.Add(item);
                                break;

                            case 2:
                                inventory.Ada1Resources.Add(item);
                                break;
                        }
                    }
                }

                i++;
            }

            return inventory;
        }

        public async Task<Stream> GetImageAsync()
        {
            var inventory = await GetInventoryAsync();

            var loader = new ImageLoader();

            Image image = Image.NewFromBuffer(ExtensionsRes.ResourcesBackground);

            using var resetRange = ImageLoader
                .RenderText($"<b>{inventory.ResetBegin.ToString("dd.MM HH:mm")} – {inventory.ResetEnd.ToString("dd.MM HH:mm")}</b>", "Arial 24", new int[] { 255, 255, 255 });
            image = image.Composite(resetRange, Enums.BlendMode.Over, 23, 87);

            int x = 22, y = 225;

            foreach (var item in inventory.SpiderResources)
            {
                image = await DrawItemAsync(loader, image, item, x, y);

                y += 135;
            }

            y = 263;

            foreach (var itemList in new List<List<ResourceItem>> { inventory.Ada1Resources, inventory.Banshee44Resources })
            {
                x = 459;

                foreach (var item in itemList)
                {
                    image = await DrawItemAsync(loader, image, item, x, y);

                    x += 433;
                }

                y += 281;
            }

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }

        internal static async Task<Image> DrawItemAsync(ImageLoader loader, Image image, ResourceItem item, int x, int y)
        {
            using var resIcon = await loader.GetImageAsync(item.ResourceIconURL);
            image = image.Composite
                (resIcon, Enums.BlendMode.Over, x, y);

            using var currencyIcon = await loader.GetImageAsync(item.ResourceCurrencyIconURL);
            image = image.Composite
                (currencyIcon.ThumbnailImage(40, 40, Enums.Size.Force), Enums.BlendMode.Over, 107 + x, 50 + y);

            using var resourceName = ImageLoader
                .RenderText(item.ResourceName, "Arial 23", new int[] { 0, 0, 0 });
            image = image.Composite(resourceName, Enums.BlendMode.Over, 109 + x, 13 + y);

            using var resourceCurrency = ImageLoader
                .RenderText(item.ResourceCurrencyQuantity, "Arial 23", new int[] { 0, 0, 0 });
            image = image.Composite(resourceCurrency, Enums.BlendMode.Over, 158 + x, 64 + y);

            return image;
        }
    }
}
