using BungieNetApi;
using BungieNetApi.Enums;
using CommonData.Localization;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class WeeklyMilestone : IWrapper
    {
        public record Field
        {
            public string Name { get; internal set; }
            public string Value { get; internal set; }
        }

        public IEnumerable<Field> Fields { get; private set; }

        public string NightfallImageURL { get; private set; }

        public bool IsIronBannerAvailable { get; private set; }

        public readonly string IronBannerImageURL = "https://bungie.net/common/destiny2_content/icons/0ee91b79ba1366243832cf810afc3b75.jpg";
        public readonly string IronBannerName = Translation.StatsActivityNames[ActivityType.IronBannerControl][0];

        private readonly IApiClient _apiClient;

        internal WeeklyMilestone(IApiClient apiClient) => _apiClient = apiClient;

        public async Task InitAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

            var ibBillboard = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ib-billboard\"]/div[2]");

            IsIronBannerAvailable = ibBillboard is not null;

            var milestone = await _apiClient.GetMilestonesAsync();

            NightfallImageURL = milestone.NightfallTheOrdealImage;

            var mode = Translation.StatsActivityNames.FirstOrDefault(x => x.Value[1].ToLower() == milestone.CrucibleRotationModeName.ToLower()).Value;

            Fields = new List<Field>
            {
                new Field
                {
                    Name = Translation.StatsActivityNames[ActivityType.ScoredNightfall][0],
                    Value = milestone.NightfallTheOrdealName,
                },
                new Field
                {
                    Name = "Ротація горнила",
                    Value = $"{mode[0]} | {mode[1]}",
                }
            };
        }
    }
}
