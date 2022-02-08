using DestinyInfocardsDatabase.ORM.LostSectors;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        /*
         * namespace DataProcessor.Parsers.Inventory
{
    public record ResourcesInventory
    {
        public DateTime ResetBegin { get; set; }

        public DateTime ResetEnd { get; set; }

        public List<ResourceItem> SpiderResources { get; set; } = new();

        public List<ResourceItem> Banshee44Resources { get; set; } = new();

        public List<ResourceItem> Ada1Resources { get; set; } = new();
    }

    public record ResourceItem
    {
        public string ResourceName { get; set; }

        public string ResourceIconURL { get; set; }

        public string ResourceCurrencyQuantity { get; set; }

        public string ResourceCurrencyIconURL { get; set; }
    }
}
*/

        public async Task<LostSectorsDailyReset> ParseResourcesAsync()
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
//using SixLabors.ImageSharp.Processing;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace DataProcessor.Parsers
//{
//    public class ResourcesParser : IInventoryParser<ResourcesInventory>
//    {
//        public async Task<ResourcesInventory> GetInventoryAsync()
//        {
//            var inventory = new ResourcesInventory();

//            var currDate = DateTime.UtcNow;
//            var resetTime = currDate.Date.AddHours(17).ToLocalTime();

//            if (currDate.Hour < 17)
//            {
//                inventory.ResetBegin = resetTime.AddDays(-1);
//                inventory.ResetEnd = resetTime;
//            }
//            else
//            {
//                inventory.ResetBegin = resetTime;
//                inventory.ResetEnd = resetTime.AddDays(1);
//            }

//            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/vendors");

//            int[][] vendorsInt = { new int[] { 3, 1, 1 }, new int[] { 7, 2, 2 } };
//            string[] vendors = { "//*[@id=\"vendors_863940356_content\"]/div[3]/div", "//*[@id=\"vendors_672118013_content\"]/div[4]/div", "//*[@id=\"vendors_350061650_content\"]/div[4]/div" };

//            int i = 0;

//            foreach (var vendor in vendors)
//            {
//                var container = htmlDoc.DocumentNode.SelectSingleNode(vendor);

//                if (container is not null)
//                {
//                    for (int j = vendorsInt[0][i]; j <= vendorsInt[1][i]; j++)
//                    {
//                        var item = new ResourceItem
//                        {
//                            ResourceName = container.SelectSingleNode($"./div[{j}]/div[3]/div[1]/p")
//                            .InnerText.Replace("Purchase ", ""),

//                            ResourceIconURL = (container.SelectSingleNode($"./div[{j}]/div[1]/img[2]") ??
//                            container.SelectSingleNode($"./div[{j}]/div[1]/img"))
//                            .Attributes["src"].Value,

//                            ResourceCurrencyQuantity =
//                            (container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[3]/div/p[2]") ??
//                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[4]/div/p[2]") ??
//                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[5]/div/p[2]"))
//                            .InnerText,

//                            ResourceCurrencyIconURL =
//                            (container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[3]/div/div/img") ??
//                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[4]/div/div/img") ??
//                            container.SelectSingleNode($"./div[{j}]/div[2]/div[2]/div[5]/div/div/img"))
//                            .Attributes["src"].Value
//                        };

//                        switch (i)
//                        {
//                            case 0:
//                                inventory.SpiderResources.Add(item);
//                                break;

//                            case 1:
//                                inventory.Banshee44Resources.Add(item);
//                                break;

//                            case 2:
//                                inventory.Ada1Resources.Add(item);
//                                break;
//                        }
//                    }
//                }

//                i++;
//            }

//            return inventory;
//        }