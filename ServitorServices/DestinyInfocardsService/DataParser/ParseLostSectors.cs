using DestinyInfocardsDatabase.ORM.LostSectors;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal static partial class DataParser
    {
        public static async Task<LostSectorsDailyReset> ParseLostSectorsAsync(DateTime dailyResetBegin, DateTime dailyResetEnd)
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
                DailyResetBegin = dailyResetBegin,
                DailyResetEnd = dailyResetEnd,
                LostSectors = sectors.ToList()
            };
        }
    }
}
