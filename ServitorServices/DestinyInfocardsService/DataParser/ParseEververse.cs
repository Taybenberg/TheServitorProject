using DestinyInfocardsDatabase.ORM.LostSectors;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    /*
     * namespace DataProcessor.Parsers.Inventory
{
    public record EververseInventory
    {
        public string Week { get; set; }

        public DateTime WeekBegin { get; set; }

        public DateTime WeekEnd { get; set; }

        public string SeasonIconURL { get; set; }

        public List<List<EververseItem>> EververseItems { get; set; } = new();
    }

    public record EververseItem
    {
        public string Icon1URL { get; set; }

        public string Icon2URL { get; set; }
    }
}
*/

    internal partial class DataParser
    {
        public async Task<LostSectorsDailyReset> ParseEververseAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            var sectorNodes = new string[] { "//*[contains(@id,'bl_lost_sector_legend')]", "//*[contains(@id,'bl_lost_sector_master')]" };

            var sectors = sectorNodes.Select(x =>
            {
                var node = htmlDoc.DocumentNode.SelectSingleNode(x);

                var lightLevel = node.SelectSingleNode("./div[14]/div[1]").InnerText;
                var sectorImageURL = node.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;
                var sectorName = node.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;
                var sectorReward = node.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                return new LostSector
                {
                    Name = sectorName,
                    Reward = sectorReward,
                    LightLevel = lightLevel,
                    ImageURL = sectorImageURL
                };
            });

            return new LostSectorsDailyReset
            {
                LostSectors = sectors.ToList()
            };
        }
    }
}
//using DataProcessor.Parsers.Inventory;
//using HtmlAgilityPack;
//using SixLabors.Fonts;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Drawing.Processing;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace DataProcessor.Parsers
//{
//    public class EververseParser : IInventoryParser<EververseInventory>
//    {
//        private readonly Lazy<Task<HtmlDocument>> htmlDocument;

//        private readonly string _seasonName;
//        private readonly DateTime _seasonStart;
//        private readonly int _weekNumber;

//        public EververseParser(string seasonName, DateTime seasonStart, int weekNumber)
//        {
//            _seasonName = seasonName;

//            _seasonStart = seasonStart;

//            _weekNumber = weekNumber;

//            htmlDocument = new Lazy<Task<HtmlDocument>>(async () => await new HtmlWeb()
//           .LoadFromWebAsync("https://www.todayindestiny.com/eververseCalendar"));
//        }

//        public async Task<EververseInventory> GetInventoryAsync() => await GetInventoryAsync(null);

//        public async Task<EververseInventory> GetInventoryAsync(int? weekNumber)
//        {
//            var inventory = new EververseInventory();

//            var htmlDoc = await htmlDocument.Value;

//            int week = weekNumber ?? _weekNumber;

//            inventory.WeekBegin = _seasonStart.AddDays((week - 1) * 7).ToLocalTime();
//            inventory.WeekEnd = inventory.WeekBegin.AddDays(7);

//            inventory.Week = $"Тиждень {week}. Сезон \"{_seasonName}\"";

//            var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div[1]/div[{week - 15}]");

//            if (eververseWeekly is not null)
//            {
//                inventory.SeasonIconURL = eververseWeekly.SelectSingleNode($"./div[1]/img").Attributes["src"].Value;

//                for (int i = 1; i <= 4; i++)
//                {
//                    var container = eververseWeekly.SelectSingleNode($"./div[2]/div/div[{i}]/div[2]/div")
//                       ?? eververseWeekly.SelectSingleNode($"./div[2]/div/div[{i}]/div[1]/div");

//                    if (container is not null)
//                    {
//                        List<EververseItem> items = new();

//                        for (int j = 1; j <= 7; j++)
//                        {
//                            var item = new EververseItem();

//                            var node = container.SelectSingleNode($"./div[{j}]/div[1]/img[3]")
//                            ?? container.SelectSingleNode($"./div[{j}]/div[1]/img[2]");

//                            if (node is not null)
//                                item.Icon1URL = node.Attributes["src"].Value;

//                            node = container.SelectSingleNode($"./div[{j}]/div[1]/img[1]");

//                            if (node is null)
//                                break;

//                            item.Icon2URL = node.Attributes["src"].Value;

//                            items.Add(item);
//                        }

//                        inventory.EververseItems.Add(items);
//                    }
//                }
//            }

//            return inventory;
//        }