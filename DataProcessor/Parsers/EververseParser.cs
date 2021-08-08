using DataProcessor.Parsers.Inventory;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor.Parsers
{
    public class EververseParser : IInventoryParser<EververseInventory>
    {
        private readonly Lazy<Task<HtmlDocument>> htmlDocument;

        private readonly string _seasonName;
        private readonly DateTime _seasonStart;
        private readonly int _weekNumber;

        public EververseParser(string seasonName, DateTime seasonStart, int weekNumber)
        {
            _seasonName = seasonName;

            _seasonStart = seasonStart;

            _weekNumber = weekNumber;

            htmlDocument = new Lazy<Task<HtmlDocument>>(async () => await new HtmlWeb()
           .LoadFromWebAsync("https://www.todayindestiny.com/eververseWeekly"));
        }

        public async Task<EververseInventory> GetInventoryAsync() => await GetInventoryAsync(null);

        public async Task<EververseInventory> GetInventoryAsync(int? weekNumber)
        {
            var inventory = new EververseInventory();

            var htmlDoc = await htmlDocument.Value;

            int week = weekNumber ?? _weekNumber;

            inventory.WeekBegin = _seasonStart.AddDays((week - 1) * 7).ToLocalTime();
            inventory.WeekEnd = inventory.WeekBegin.AddDays(7);

            inventory.Week = $"Тиждень {week}. Сезон \"{_seasonName}\"";

            var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div/div[{week}]");

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

            return inventory;
        }

        public async Task<Stream> GetImageAsync() => await GetImageAsync(null);

        public async Task<Stream> GetImageAsync(int? weekNumber)
        {
            var inventory = await GetInventoryAsync(weekNumber);

            var loader = new ImageLoader();

            using Image image = Image.Load(ExtensionsRes.EververseItemsBackground);

            Font font = new Font(SystemFonts.Find("Arial"), 30, FontStyle.Bold);

            using Image icon = await loader.GetImageAsync(inventory.SeasonIconURL);

            icon.Mutate(m => m.Resize(192, 192));

            image.Mutate(m =>
            {
                m.DrawText(inventory.Week, font, Color.White, new Point(212, 12));

                m.DrawText
                    ($"{inventory.WeekBegin.ToString("dd.MM HH:mm")} – {inventory.WeekEnd.ToString("dd.MM HH:mm")}",
                    font, Color.White, new Point(212, 73));

                m.DrawImage(icon, new Point(0, 0), 1);
            });

            int[] Y = { 178, 361, 467, 650 };

            int i = 0;

            foreach (var itemList in inventory.EververseItems)
            {
                int x = 35, y = Y[i++];

                foreach (var item in itemList)
                {
                    await DrawItemAsync(item, loader, image, x, y);

                    x += 106;
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }

        internal static async Task DrawItemAsync(EververseItem item, ImageLoader loader, Image image, int x, int y)
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
        }

        public async Task<Stream> GetFullInventoryAsync(DateTime seasonEnd)
        {
            int weeksTotal = (int)(seasonEnd - _seasonStart).TotalDays / 7;

            int rows = (int)Math.Sqrt(weeksTotal);
            int columns = (int)Math.Ceiling((double)weeksTotal / rows);

            Stream[] streams = new Stream[weeksTotal];

            Parallel.For(0, weeksTotal, (i) =>
            {
                streams[i] = GetImageAsync(i + 1).Result;
            });

            using var image = new Image<Rgba32>(columns * 802, rows * 902);

            int week = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (week < weeksTotal)
                    {
                        using var str = streams[week];

                        using var img = Image.Load(str);

                        image.Mutate(m => m.DrawImage(img, new Point(j * 802, i * 902), 1));
                    }

                    week++;
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
