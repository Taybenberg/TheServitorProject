using BungieNetApi.Enums;
using System.Web;
using static BungieNetApi.Enums.ActivityType;

namespace DataProcessor
{
    class CumulativeActivityCounter
    {
        private static readonly object obj = new object();

        public int[] Count { get; private set; } = new int[3];

        public string QuickChartURL
        {
            get
            {
                var quickChartString = "{\"type\":\"outlabeledPie\",\"data\":" +
                "{\"labels\":[\"ПвЕ\",\"ПвП\",\"ПвПвЕ\"],\"datasets\":" +
                "[{\"backgroundColor\":[\"#f9a825\",\"#ff5722\",\"#81c784\"]," +
                "\"data\":[" + string.Join(",", Count) + "]}]}," +
                "\"options\":{\"plugins\":{\"legend\":false,\"outlabels\":" +
                "{\"text\":\"%l %p\",\"color\":\"white\",\"stretch\":35," +
                "\"font\":{\"resizable\":true,\"minSize\":16,\"maxSize\":18}}}}}";

                return $"https://quickchart.io/chart?c={HttpUtility.UrlEncode(quickChartString)}";
            }
        }

        public void Add(ActivityType activityType, int count)
        {
            lock (obj)
            {
                Count[ActivityTypeDefiner(activityType)] += count;
            }
        }

        public int ActivityTypeDefiner(ActivityType activityType)
        {
            switch (activityType)
            {
                case Gambit:
                case GambitPrime:
                    return 2;

                case PrivateMatchesAll:
                case PrivateMatchesClash:
                case PrivateMatchesControl:
                case PrivateMatchesSupremacy:
                case PrivateMatchesCountdown:
                case PrivateMatchesSurvival:
                case PrivateMatchesMayhem:
                case PrivateMatchesRumble:
                case AllPvP:
                case AllMayhem:
                case CrimsonDoubles:
                case AllDoubles:
                case Doubles:
                case Salvage:
                case Elimination:
                case Momentum:
                case Supremacy:
                case Countdown:
                case Showdown:
                case Rumble:
                case Lockdown:
                case Scorched:
                case ScorchedTeam:
                case Breakthrough:
                case IronBanner:
                case IronBannerControl:
                case IronBannerClash:
                case IronBannerSupremacy:
                case IronBannerSalvage:
                case Survival:
                case PvPCompetitive:
                case PvPQuickplay:
                case Clash:
                case ClashQuickplay:
                case ClashCompetitive:
                case Control:
                case ControlQuickplay:
                case ControlCompetitive:
                case TrialsOfOsiris:
                case TrialsOfTheNine:
                case TrialsCountdown:
                case TrialsSurvival:
                    return 1;

                default:
                    return 0;
            }
        }
    }
}
