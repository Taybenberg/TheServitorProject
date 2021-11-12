using DataProcessor.Parsers.Inventory;
using HtmlAgilityPack;
using NetVips;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
           .LoadFromWebAsync("https://www.todayindestiny.com/eververseCalendar"));
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

            var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div[1]/div[{week}]");

            if (eververseWeekly is not null)
            {
                inventory.SeasonIconURL = eververseWeekly.SelectSingleNode($"./div[1]/img").Attributes["src"].Value;

                for (int i = 1; i <= 4; i++)
                {
                    var container = eververseWeekly.SelectSingleNode($"./div[2]/div/div[{i}]/div[2]/div")
                       ?? eververseWeekly.SelectSingleNode($"./div[2]/div/div[{i}]/div[1]/div");

                    if (container is not null)
                    {
                        List<EververseItem> items = new();

                        for (int j = 1; j <= 7; j++)
                        {
                            var item = new EververseItem();

                            var node = container.SelectSingleNode($"./div[{j}]/div[1]/img[3]")
                            ?? container.SelectSingleNode($"./div[{j}]/div[1]/img[2]");

                            if (node is not null)
                                item.Icon1URL = node.Attributes["src"].Value;

                            node = container.SelectSingleNode($"./div[{j}]/div[1]/img[1]");

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
            var image = await DrawImageAsync(weekNumber);

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }

        internal async Task<Image> DrawImageAsync(int? weekNumber)
        {
            var inventory = await GetInventoryAsync(weekNumber);

            var loader = new ImageLoader();

            Image image = Image.NewFromBuffer(ExtensionsRes.EververseItemsBackground);

            using var week = ImageLoader
                .RenderText($"<b>{inventory.Week}</b>", "Arial 30", new int[] { 255, 255, 255 });
            image = image.Composite(week, Enums.BlendMode.Over, 213, 17);

            using var weekBegin = ImageLoader
                .RenderText
                ($"<b>{inventory.WeekBegin.ToString("dd.MM HH:mm")} – {inventory.WeekEnd.ToString("dd.MM HH:mm")}</b>",
                "Arial 30", new int[] { 255, 255, 255 });
            image = image.Composite(weekBegin, Enums.BlendMode.Over, 213, 78);

            using var icon = await loader.GetImageAsync(inventory.SeasonIconURL);
            image = image.Composite
                (icon.ThumbnailImage(192, 192, Enums.Size.Force), Enums.BlendMode.Over, 0, 0);

            int[] Y = { 178, 361, 467, 650 };

            int i = 0;

            foreach (var itemList in inventory.EververseItems)
            {
                int x = 35, y = Y[i++];

                foreach (var item in itemList)
                {
                    image = await DrawItemAsync(loader, image, item, x, y);

                    x += 106;
                }
            }

            return image;
        }

        internal static async Task<Image> DrawItemAsync(ImageLoader loader, Image image, EververseItem item, int x, int y)
        {
            if (item.Icon1URL is not null)
            {
                using var icon1 = await loader.GetImageAsync(item.Icon1URL);
                image = image.Composite(icon1, Enums.BlendMode.Over, x, y);
            }

            if (item.Icon2URL is not null)
            {
                using var icon2 = await loader.GetImageAsync(item.Icon2URL);
                image = image.Composite(icon2, Enums.BlendMode.Over, x, y);
            }

            return image;
        }

        public async Task<Stream> GetFullInventoryAsync(DateTime seasonEnd)
        {
            int weeksTotal = (int)(seasonEnd - _seasonStart).TotalDays / 7 + 1;

            var tasks = Enumerable.Range(1, weeksTotal).Select(i => Task.Run(async () => await DrawImageAsync(i))).ToArray();

            int rows = (int)Math.Sqrt(weeksTotal);
            int columns = (int)Math.Ceiling((double)weeksTotal / rows);

            var image = Image.Black(columns * 802, rows * 902);

            int week = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (week < weeksTotal)
                    {
                        using var img = await tasks[week];
                        image = image.Insert(img, j * 802, i * 902);
                    }

                    week++;
                }
            }

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
