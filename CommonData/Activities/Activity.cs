using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using CommonData.DiscordEmoji;
using CommonData.Localization;
using static BungieSharper.Entities.Destiny.HistoricalStats.Definitions.DestinyActivityModeType;

namespace CommonData.Activities
{
    public partial class Activity
    {
        public static (string emoji, string activityTitle) GetActivityInfo(DestinyActivityModeType activityType, string activityName)
        {
            string emoji, activityTitle;

            switch (activityType)
            {
                case Raid:
                    {
                        var raid = GetRaidType(activityName ?? string.Empty);
                        emoji = Emoji.GetActivityRaidEmoji(raid);
                        activityTitle = Translation.ActivityRaidTypes[raid] ?? activityName ?? Translation.ActivityNames[activityType][0];
                    }
                    break;

                default:
                    {
                        emoji = Emoji.GetActivityEmoji(activityType);
                        activityTitle = activityName ?? Translation.ActivityNames[activityType][0];
                    }
                    break;
            }

            return (emoji, activityTitle);
        }

        public static int GetFireteamSize(DestinyActivityModeType activityType) =>
            activityType switch
            {
                Raid or
                Menagerie or
                AllPvP or
                AllMayhem or
                Supremacy or
                IronBannerControl or
                ScorchedTeam or
                ControlQuickplay or
                VexOffensive or
                Momentum or
                Sundial or
                Dares => 6,

                TrialsOfTheNine or
                Lockdown or
                Countdown or
                Breakthrough or
                ClashQuickplay or
                Reckoning or
                Gambit or
                GambitPrime => 4,

                Rumble => 1,

                _ => 3
            };

        public static DestinyActivityModeType[] QuickActivityTypes =>
            new DestinyActivityModeType[]
            {
                Dungeon,
                Gambit,
                TrialsOfOsiris,
                IronBannerControl,
                Survival,
                AllPvP,
                ScoredNightfall,
                AllPvE
            };
    }
}
