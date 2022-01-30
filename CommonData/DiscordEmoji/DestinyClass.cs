using BungieSharper.Entities.Destiny;
using static BungieSharper.Entities.Destiny.DestinyClass;

namespace CommonData.DiscordEmoji
{
    public static partial class Emoji
    {
        public static string GetClassEmoji(DestinyClass destinyClass) =>
            destinyClass switch
            {
                Titan => "<:TitanLogo:865673798408863764>",
                Hunter => "<:HunterLogo:865673798014730271>",
                Warlock => "<:WarlockLogo:865674599919648810>",
                _ => DefaultD2
            };
    }
}
