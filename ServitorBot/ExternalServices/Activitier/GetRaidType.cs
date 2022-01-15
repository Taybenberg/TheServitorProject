using CommonData.DiscordEmoji;
using CommonData.Localization;
using CommonData.RaidManager;
using Discord;
using Discord.WebSocket;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private RaidType GetRaidType(string raidType) =>
            raidType.ToLower() switch
            {
                "lw" or "лв" or "об" => RaidType.LW,
                "gos" or "сп" or "сс" => RaidType.GOS,
                "dsc" or "сгк" => RaidType.DSC,
                "vog" or "вог" or "кс" => RaidType.VOG_L,
                "vogm" or "вогм" or "ксм" => RaidType.VOG_M,
                _ => RaidType.Undefined
            };
    }
}