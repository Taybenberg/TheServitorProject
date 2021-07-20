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
    public class EververseParser : IInventoryParser<EververseInventory>
    {
        private string _seasonName;
        private DateTime _seasonStart;
        private int _weekNumber;

        public EververseParser(string seasonName, DateTime seasonStart, int weekNumber) =>
            (_seasonName, _seasonStart, _weekNumber) = (seasonName, seasonStart, weekNumber);

        public async Task<EververseInventory> GetInventoryAsync()
        {
            var inventory = new EververseInventory();

            inventory.WeekBegin = _seasonStart.AddDays((_weekNumber - 1) * 7).ToLocalTime();
            inventory.WeekEnd = inventory.WeekBegin.AddDays(7);

            inventory.Week = $"Тиждень {_weekNumber}. Сезон \"{_seasonName}\"";

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseWeekly");
            var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div/div[{_weekNumber}]");

            if (eververseWeekly is not null)
            {
                inventory.SeasonIconURL = eververseWeekly.SelectSingleNode($"./div[1]/img").Attributes["src"].Value;

                for (int i = 1; i <= 4; i++)
                {
                    var container = eververseWeekly.SelectSingleNode($"./div[2]/div[{i}]/div");

                    if (container is not null)
                    {
                        List<EververseItem> items = new();

                        for (int j = 1; j <= 7; j++)
                        {
                            var item = new EververseItem();

                            var node = container.SelectSingleNode($"./div[{j}]/div[1]/div/img[3]")
                            ?? container.SelectSingleNode($"./div[{j}]/div[1]/div/img[2]");

                            if (node is not null)
                                item.Icon1URL = node.Attributes["src"].Value;

                            node = container.SelectSingleNode($"./div[{j}]/div[1]/div/img[1]");

                            if (node is null)
                                break;

                            item.Icon2URL = node.Attributes["src"].Value;

                            items.Add(item);
                        }

                        inventory.EververseItems.Add(items);
                    }
                }
            }
            else
                return null;

            return inventory;
        }

        public async Task<Stream> GetImageAsync()
        {
            var inventory = await GetInventoryAsync();

            var loader = new ImageLoader();

            using Image image = Image.Load(ExtensionsRes.EververseItemsBackground);

            if (inventory is not null)
            {
                Font font = new Font(SystemFonts.Find("Arial"), 30, FontStyle.Bold);

                using Image icon = await loader.GetImageAsync(inventory.SeasonIconURL);

                icon.Mutate(m => m.Resize(192, 192));

                image.Mutate(m =>
                {
                    m.DrawText(inventory.Week, font, Color.White, new Point(212, 12));

                    m.DrawText
                        ($"{inventory.WeekBegin.ToString("dd.MM HH:mm")} – {inventory.WeekEnd.ToString("dd.MM HH:mm")}",
                        font, Color.White, new Point(212, 73));

                    image.Mutate(m => m.DrawImage(icon, new Point(0, 0), 1));
                });

                int[] Y = { 178, 361, 467, 650 };

                int i = 0;

                foreach (var itemList in inventory.EververseItems)
                {
                    int x = 35, y = Y[i++];

                    foreach (var item in itemList)
                    {
                        if (item.Icon1URL is not null)
                        {
                            using Image icon1 = await loader.GetImageAsync(item.Icon1URL);

                            image.Mutate(m => m.DrawImage(icon1, new Point(x, y), 1));
                        }

                        if (item.Icon2URL is not null)
                        {
                            using Image icon2 = await loader.GetImageAsync(item.Icon2URL);

                            image.Mutate(m => m.DrawImage(icon2, new Point(x, y), 1));
                        }

                        x += 106;
                    }
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
