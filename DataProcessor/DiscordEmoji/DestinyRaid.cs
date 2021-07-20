﻿using DataProcessor.RaidManager;
using static DataProcessor.RaidManager.RaidType;

namespace DataProcessor.DiscordEmoji
{
    public static partial class EmojiContainer
    {
        public static string GetRaidEmoji(RaidType raidType) =>
            raidType switch
            {
                LW => "<:LW:867048825821593622>",
                GOS => "<:GOS:867048826049265674>",
                DSC => "<:DSC:867048825818447902>",
                VOG_L => "<:VOG_L:867050674927697950>",
                VOG_M => "<:VOG_M:867050675041337344>",
                _ => NoData
            };
    }
}