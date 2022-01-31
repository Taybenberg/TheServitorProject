using System.Web;

namespace ClanActivitiesService.Containers
{
    public class UserPartnersContainer
    {
        public IEnumerable<PartnersCounter> Partners { get; internal set; }

        public int TotalCount { get; internal set; }

        public int CoopCount { get; internal set; }

        public string UserName { get; internal set; }

        public string ChartImageURL => GetChart();

        internal IEnumerable<(string username, int[] count)> TopPartners { get; set; }

        private string GetChart()
        {
            var quickChartString = "{type:'radar',data:{labels:[" +
                         string.Join(',', TopPartners.Select(x => $"'{x.username}'")) +
                        "],datasets:[{label:'ПвЕ',borderColor:'#7986cb',pointBackgroundColor:'#7986cb',data:[" +
                        string.Join(',', TopPartners.Select(x => x.count[0])) + "],fill:false}," +
                        "{label:'ПвП',borderColor:'#ff7043',pointBackgroundColor:'#ff7043',data:[" +
                        string.Join(',', TopPartners.Select(x => x.count[1])) + "],fill:false}," +
                        "{label:'ПвЕвП',borderColor:'#81c784',pointBackgroundColor:'#81c784',data:[" +
                        string.Join(',', TopPartners.Select(x => x.count[2])) +
                        "],fill:false}],},options:{legend:{labels:{fontColor:'white'}},scale:{angleLines:{color:" +
                        "'rgba(255,255,255,0.5)'},ticks:{fontColor:'white',backdropColor:'transparent'},gridLines:{" +
                        "color:'rgba(255,255,255,0.5)',},pointLabels:{fontColor:'white'}}}}";

            return $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
        }
    }
}
