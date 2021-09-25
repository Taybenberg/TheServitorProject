using DataProcessor.Parsers.Inventory;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
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

            using Image image = Image.Load(ExtensionsRes.ResourcesBackground);

            Font dateFont = new Font(SystemFonts.Find("Arial"), 24, FontStyle.Bold);

            image.Mutate(m => m
                .DrawText($"{inventory.ResetBegin.ToString("dd.MM HH:mm")} – {inventory.ResetEnd.ToString("dd.MM HH:mm")}",
                dateFont, Color.White, new Point(21, 81)));

            Font font = new Font(SystemFonts.Find("Arial"), 23, FontStyle.Regular);

            int x = 22, y = 225;

            foreach (var item in inventory.SpiderResources)
            {
                await DrawItemAsync(item, loader, image, font, x, y);

                y += 135;
            }

            y = 263;

            foreach (var itemList in new List<List<ResourceItem>> { inventory.Ada1Resources, inventory.Banshee44Resources })
            {
                x = 459;

                foreach (var item in itemList)
                {
                    await DrawItemAsync(item, loader, image, font, x, y);

                    x += 433;
                }

                y += 281;
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }

        internal static async Task DrawItemAsync(ResourceItem item, ImageLoader loader, Image image, Font font, int x, int y)
        {
            using Image currencyIcon = await loader.GetImageAsync(item.ResourceCurrencyIconURL);

            currencyIcon.Mutate(m => m.Resize(40, 40));

            using Image resIcon = await loader.GetImageAsync(item.ResourceIconURL);

            image.Mutate(m =>
            {
                m.DrawImage(resIcon, new Point(x, y), 1);

                m.DrawText(item.ResourceName, font, Color.Black, new Point(107 + x, 7 + y));

                m.DrawImage(currencyIcon, new Point(107 + x, 50 + y), 1);

                m.DrawText(item.ResourceCurrencyQuantity, font, Color.Black, new Point(156 + x, 58 + y));
            });
        }
    }
}
