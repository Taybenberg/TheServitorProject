using DestinyInfocardsDatabase.ORM.LostSectors;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        /*
         * namespace DataProcessor.Parsers.Inventory
{
    public record RoadmapInventory
    {
        public byte[] RoadmapImage { get; set; }
    }
}
*/

        public async Task<LostSectorsDailyReset> ParseWeeklyMilestoneAsync()
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
//using BungieNetApi;
//using BungieNetApi.Enums;
//using CommonData.Localization;
//using HtmlAgilityPack;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DataProcessor.DatabaseWrapper
//{
//    public class WeeklyMilestone : IWrapper
//    {
//        public record Field
//        {
//            public string Name { get; internal set; }
//            public string Value { get; internal set; }
//        }

//        public IEnumerable<Field> Fields { get; private set; }

//        public string NightfallImageURL { get; private set; }

//        public bool IsIronBannerAvailable { get; private set; }

//        public readonly string IronBannerImageURL = "https://bungie.net/common/destiny2_content/icons/0ee91b79ba1366243832cf810afc3b75.jpg";
//        public readonly string IronBannerName = Translation.StatsActivityNames[ActivityType.IronBannerControl][0];

//        private readonly IApiClient _apiClient;

//        internal WeeklyMilestone(IApiClient apiClient) => _apiClient = apiClient;

//        public async Task InitAsync()
//        {
//            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

//            var ibBillboard = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ib-billboard\"]/div[2]");

//            IsIronBannerAvailable = ibBillboard is not null;

//            var milestone = await _apiClient.GetMilestonesAsync();

//            NightfallImageURL = milestone.NightfallTheOrdealImage;

//            var mode = Translation.StatsActivityNames.FirstOrDefault(x => x.Value[1].ToLower() == milestone.CrucibleRotationModeName.ToLower()).Value;

//            Fields = new List<Field>
//            {
//                new Field
//                {
//                    Name = Translation.StatsActivityNames[ActivityType.ScoredNightfall][0],
//                    Value = milestone.NightfallTheOrdealName,
//                },
//                new Field
//                {
//                    Name = "Ротація горнила",
//                    Value = $"{mode[0]} | {mode[1]}",
//                }
//            };
//        }
//    }
//}
