﻿using BungieNetApi.Enums;
using static BungieNetApi.Enums.DestinyClass;

namespace DataProcessor.DiscordEmoji
{
    public static partial class EmojiContainer
    {
        public static string GetClassEmoji(DestinyClass destinyClass) =>
            destinyClass switch
            {
                Titan => "<:TitanLogo:865673798408863764>",
                Hunter => "<:HunterLogo:865673798014730271>",
                Warlock => "<:WarlockLogo:865674599919648810>",
                _ => NoData
            };
    }
}
