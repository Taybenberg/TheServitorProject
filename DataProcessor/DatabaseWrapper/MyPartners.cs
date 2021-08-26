using BungieNetApi.Enums;
using Database;
using Database.ORM;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DataProcessor.DatabaseWrapper
{
    public class MyPartners : IWrapper
    {
        public record PartnerCounter
        {
            public string UserName { get; internal set; }
            public int Count { get; internal set; }
        }

        public bool IsUserRegistered { get; private set; }

        public string UserName { get; private set; }

        public string QuickChartURL { get; private set; }

        public IEnumerable<PartnerCounter> Partners { get; private set; }

        private readonly ulong _userID;

        private readonly IClanDB _clanDB;

        internal MyPartners(IClanDB clanDB, ulong discordUserID) => (_clanDB, _userID) = (clanDB, discordUserID);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserWithActivitiesAndOtherUserStatsAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            UserName = user.UserName;

            var acts = user.Characters.SelectMany(x => x.ActivityUserStats.Select(y => y.Activity)).Distinct();

            var users = (await _clanDB.GetUsersWithCharactersAsync()).Where(x => x.UserID != user.UserID);

            ConcurrentBag<(string userName, IEnumerable<Activity> activities, int count)> counter = new();

            Parallel.ForEach(users, (usr) =>
            {
                var activities = acts.Where(x => x.ActivityUserStats.Any(y => usr.Characters.Any(c => c.CharacterID == y.CharacterID)));

                var count = activities.Count();

                if (count > 0)
                    counter.Add((usr.UserName, activities, count));
            });

            var partners = counter.OrderByDescending(x => x.Item3);

            Partners = partners.Select(x => new PartnerCounter
            {
                UserName = x.userName,
                Count = x.count
            });

            ConcurrentBag<(string username, CumulativeActivityCounter counter)> userCumulativeCounter = new();

            Parallel.ForEach(partners.Take(10), (partner) =>
            {
                var cumulativeCounter = new CumulativeActivityCounter();

                Parallel.ForEach((ActivityType[])Enum.GetValues(typeof(ActivityType)), (type) =>
                {
                    var count = partner.activities.Count(x => x.ActivityType == type);

                    if (count > 0)
                        cumulativeCounter.Add(type, count);
                });

                userCumulativeCounter.Add((partner.userName, cumulativeCounter));
            });

            var quickChartString = "{type:'radar',data:{labels:[" +
             string.Join(',', userCumulativeCounter.Select(x => $"'{x.username}'")) +
            "],datasets:[{label:'ПвЕ',borderColor:'#7986cb',pointBackgroundColor:'#7986cb',data:[" +
            string.Join(',', userCumulativeCounter.Select(x => x.counter.Count[0])) + "],fill:false}," +
            "{label:'ПвП',borderColor:'#ff7043',pointBackgroundColor:'#ff7043',data:[" +
            string.Join(',', userCumulativeCounter.Select(x => x.counter.Count[1])) + "],fill:false}," +
            "{label:'ПвЕвП',borderColor:'#81c784',pointBackgroundColor:'#81c784',data:[" +
            string.Join(',', userCumulativeCounter.Select(x => x.counter.Count[2])) +
            "],fill:false}],},options:{legend:{labels:{fontColor:'white'}},scale:{angleLines:{color:" +
            "'rgba(255,255,255,0.5)'},ticks:{fontColor:'white',backdropColor:'transparent'},gridLines:{" +
            "color:'rgba(255,255,255,0.5)',},pointLabels:{fontColor:'white'}}}}";

            QuickChartURL = $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
        }
    }
}
