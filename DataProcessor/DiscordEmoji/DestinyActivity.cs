using BungieNetApi.Enums;
using static BungieNetApi.Enums.ActivityType;

namespace DataProcessor.DiscordEmoji
{
    public static partial class EmojiContainer
    {
        public static string GetActivityEmoji(ActivityType activityType) =>
            activityType switch
            {
                Patrol => "<:Patrol:865683931752038450>",
                Story => "<:Story:865685179531395102>",
                Dungeon => "<:Dungeon:865689171322601484>",
                Raid => "<:Raid:865691862891364382>",
                Gambit => "<:Gambit:865722826019569675>",
                NightmareHunt => "<:Shadowkeep:865723764834697216>",
                TrialsOfOsiris => "<:Trials:865724248017862766>",
                IronBannerControl => "<:IronBanner:865870290814238783>",
                PrivateMatchesAll => "<:PrivateMatch:865875272003223552>",
                Survival => "<:Survival:865878788974182420>",
                ControlQuickplay => "<:Control:865880144581361675>",
                Elimination => "<:Elimination:865881701627461642>",
                Rumble => "<:Rumble:865882745078546443>",
                ClashQuickplay => "<:Clash:865883690433380353>",
                AllMayhem => "<:Mayhem:865887437925908510>",
                Showdown => "<:Showdown:865889639201898516>",
                Momentum => "<:Momentum:865896180582973470>",
                ScorchedTeam => "<:TeamScorched:865897102679605308>",
                Strike => "<:StrikePlaylist:865898510930673685>",
                ScoredNightfall => "<:NightfallTheOrdeal:865899405886226483>",
                Nightfall => "<:Nightfall:865900085978857502>",
                _ => NoData
            };
    }
}
