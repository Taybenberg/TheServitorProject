using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using CommonData.Activities;
using static CommonData.Activities.Activity.ActivityRaidType;

namespace CommonData.DiscordEmoji
{
    public static partial class Emoji
    {
        public static string GetActivityRaidEmoji(Activity.ActivityRaidType raidType) =>
            raidType switch
            {
                LW => "<:LW:867048825821593622>",
                GOS => "<:GOS:867048826049265674>",
                DSC => "<:DSC:867048825818447902>",
                VOGL => "<:VOG_L:867050674927697950>",
                VOGM => "<:VOG_M:867050675041337344>",
                _ => GetActivityEmoji(DestinyActivityModeType.Raid)
            };
    }
}