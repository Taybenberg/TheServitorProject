using Discord;
using System;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        string GetTitle(MessagesEnum messagesEnum, IMessage message) => messagesEnum switch
        {
            Wait => "…",
            Leaderboard => $"БЕТА | Дошка лідерів",
            ClanStats => $"БЕТА | Статистика клану {(message.Channel as IGuildChannel).Guild.Name}",
            MyActivities => $"Активності {message.Author.Username}",
            MyPartners => $"Побратими {message.Author.Username}",
            ClanActivities => $"Активності клану {(message.Channel as IGuildChannel).Guild.Name}",
            Suspicious => $"Останні активності",
            Help => $"Допомога",
            Register or NotRegistered or AlreadyRegistered => $"Реєстрація",
            Reset => $"Тиждень {(int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1}",
            Xur => $"Зур привіз свіжий крам",
            Bip => $"біп…",
            Modes => $"Режими",
            _ => string.Empty
        };
    }
}
