using DestinyInfocardsDatabase.ORM.LostSectors;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        public async Task<LostSectorsDailyReset> ParseLostSectorsAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            var sectorNodes = new string[] { "//*[contains(@id,'bl_lost_sector_legend')]", "//*[contains(@id,'bl_lost_sector_master')]" };

            List<LostSector> lostSectors = new();

            foreach (var sectorNode in sectorNodes)
            {
                var node = htmlDoc.DocumentNode.SelectSingleNode(sectorNode);

                if (node is not null)
                {
                    var lightLevel = node.SelectSingleNode(".//*[@class='powerLevelText']").InnerText;
                    var sectorImageURL = node.SelectSingleNode(".//*[@class='d-block eventCardHeaderImage']").Attributes["src"].Value;
                    var sectorName = node.SelectSingleNode(".//*[@class='eventCardHeaderName']").InnerText;
                    var sectorReward = node.SelectSingleNode(".//*[@class='eventCardDatabaseItemName'][contains(text(),'IF SOLO')]").InnerText[10..^7];

                    lostSectors.Add(new LostSector
                    {
                        Name = sectorName,
                        Reward = sectorReward,
                        LightLevel = lightLevel,
                        ImageURL = sectorImageURL
                    });
                }
            }

            return new LostSectorsDailyReset
            {
                LostSectors = lostSectors
            };
        }
    }
}
