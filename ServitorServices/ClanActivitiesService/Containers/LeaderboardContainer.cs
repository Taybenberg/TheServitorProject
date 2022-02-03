using System.Web;

namespace ClanActivitiesService.Containers
{
    public class LeaderboardContainer
    {
        public IEnumerable<LeaderboardStat> LeaderboardStats { get; internal set; }

        public string ChartImageURL => GetChart();

        private string GetChart()
        {
            var quickChartString = "{type:'radar',data:{labels:[" + string.Join(',', LeaderboardStats.Select(x => $"'{x.StatName}'")) +
                    "],datasets:[{borderColor:'#25C486',backgroundColor:'rgba(37,196,134,0.5)',pointBackgroundColor:'#25C486'," +
                    "data:[" + string.Join(',', LeaderboardStats.Select(x => 100 - x.Leaders.First(y => y.IsCurrUser).Rank)) + "]}],}," +
                    "options:{legend:{display:false},scale:{angleLines:{color:'rgba(255,255,255,0.5)'},ticks:{display:false," +
                    "suggestedMin:0,suggestedMax:99},gridLines:{color:'rgba(255,255,255,0.5)'},pointLabels:{fontColor:'white'}}}}";

            return $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
        }
    }
}
