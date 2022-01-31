using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using System.Web;
using static BungieSharper.Entities.Destiny.HistoricalStats.Definitions.DestinyActivityModeType;

namespace ClanActivitiesService.Containers
{
    public class ModeCountersContainer
    {
        public IEnumerable<ModeCounter> Counters { get; set; }

        public int TotalCount => Counters.Sum(x => x.Count);

        public int[] TypeCounters => GetTypes();

        private int[] GetTypes()
        {
            int[] types = new int[3];

            foreach (var counter in Counters)
                types[GetType(counter.ActivityMode)] += counter.Count;

            return types;
        }

        public string ChartImageURL => GetChart();

        private string GetChart()
        {
            var types = TypeCounters;

            var quickChartString = "{type:'outlabeledPie',data:{labels:['ПвЕ','ПвП','ПвПвЕ']," +
                    "datasets:[{backgroundColor:['#7986cb','#ff7043','#81c784'],data:[" +
                    string.Join(',', types) + "]}]},options:{plugins:{'legend':false,outlabels:{text:" +
                    "'%l %p',color:'white',stretch:35,font:{resizable:true,minSize:16,maxSize:18}}}}}";

            return $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
        }

        private int GetType(DestinyActivityModeType mode) =>
            mode switch
            {
                Gambit or GambitPrime => 2,

                PrivateMatchesAll or
                PrivateMatchesClash or
                PrivateMatchesControl or
                PrivateMatchesSupremacy or
                PrivateMatchesCountdown or
                PrivateMatchesSurvival or
                PrivateMatchesMayhem or
                PrivateMatchesRumble or
                AllPvP or
                AllMayhem or
                CrimsonDoubles or
                AllDoubles or
                Doubles or
                Salvage or
                Elimination or
                Momentum or
                Supremacy or
                Countdown or
                Showdown or
                Rumble or
                Lockdown or
                Scorched or
                ScorchedTeam or
                Breakthrough or
                IronBanner or
                IronBannerControl or
                IronBannerClash or
                IronBannerSupremacy or
                IronBannerSalvage or
                Survival or
                PvPCompetitive or
                PvPQuickplay or
                Clash or
                ClashQuickplay or
                ClashCompetitive or
                Control or
                ControlQuickplay or
                ControlCompetitive or
                TrialsOfOsiris or
                TrialsOfTheNine or
                TrialsCountdown or
                TrialsSurvival => 1,

                _ => 0
            };
    }
}
