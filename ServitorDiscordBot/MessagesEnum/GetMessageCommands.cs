﻿using System.Collections.Generic;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private readonly static Dictionary<MessagesEnum, string[]> messageCommands = new()
        {
            [Bip] = new string[] { "біп", "bip" },
            [Help] = new string[] { "допомога", "help" },
            [Weekly] = new string[] { "тиждень", "weekly" },
            [Sectors] = new string[] { "сектори", "sectors" },
            [Resources] = new string[] { "ресурси", "resources" },
            [Modes] = new string[] { "режими", "modes" },
            [ClanActivities] = new string[] { "кланові активності", "clan activities" },
            [MyActivities] = new string[] { "мої активності", "my activities" },
            [MyPartners] = new string[] { "мої побратими", "my partners" },
            [Register] = new string[] { "реєстрація", "registration" },
            [_100K] = new string[] { "100к", "100k" },
            [Apostates] = new string[] { "відступники", "apostates" },
            [Xur] = new string[] { "зур", "xur" },
            [Osiris] = new string[] { "осіріс", "сосіріс", "osiris" },
            [Eververse] = new string[] { "еверверс", "eververse" },
            [NotRegistered] = new string[] { "зареєструватися", "register" },
            [ClanStats] = new string[] { "статистика клану", "clan stats" },
            [Leaderboard] = new string[] { "дошка лідерів", "leaderboard" }
        };
    }
}