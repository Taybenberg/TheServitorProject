﻿using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private readonly static Dictionary<MessagesEnum, string[]> messageCommands = new()
        {
            [Weekly] = new string[] { "тиждень", "weekly" },
            [Sectors] = new string[] { "сектори", "sectors" },
            [Resources] = new string[] { "ресурси", "resources" },
            [MyGrandmasters] = new string[] { "мої грандмайстри", "my grandmasters" },
            [MyRaids] = new string[] { "мої рейди", "my raids" },
            [MyPartners] = new string[] { "мої побратими", "my partners" },
            [Register] = new string[] { "реєстрація", "registration" },
            [_100K] = new string[] { "100к", "100k" },
            [Apostates] = new string[] { "відступники", "apostates" },
            [Xur] = new string[] { "зур", "xur" },
            [Eververse] = new string[] { "еверверс", "eververse" },
            [EververseAll] = new string[] { "еверверс все", "eververse all" },
            [NotRegistered] = new string[] { "зареєструватися", "register" },
            [ClanStats] = new string[] { "статистика клану", "clan stats" },
            [Leaderboard] = new string[] { "дошка лідерів", "leaderboard" },
        };
    }
}