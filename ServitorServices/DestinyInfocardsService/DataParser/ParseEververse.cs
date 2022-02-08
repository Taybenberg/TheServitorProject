using DestinyInfocardsDatabase.ORM.Eververse;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        public async Task<EververseInventory> ParseEververseAsync(int weekNumber)
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseCalendar");

            List<EververseItem> eververseItems = new();

            return new EververseInventory
            {
                EververseItems = eververseItems
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

//        public async Task<EververseInventory> GetInventoryAsync() => await GetInventoryAsync(null);

//        public async Task<EververseInventory> GetInventoryAsync(int? weekNumber)
//        {
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