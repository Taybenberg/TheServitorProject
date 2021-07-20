using DataProcessor.DiscordEmoji;
using Discord;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        string GetTitle(MessagesEnum messagesEnum, IMessage message) => messagesEnum switch
        {
            Wait => EmojiContainer.Loading,
            Leaderboard => $"БЕТА | Дошка лідерів",
            ClanStats => $"БЕТА | Статистика клану {(message.Channel as IGuildChannel).Guild.Name}",
            MyRaids => $"Рейди {message.Author.Username}",
            MyActivities => $"Активності {message.Author.Username}",
            MyPartners => $"Побратими {message.Author.Username}",
            ClanActivities => $"Активності клану {(message.Channel as IGuildChannel).Guild.Name}",
            Suspicious => $"Останні активності",
            Help => $"Допомога",
            Register or NotRegistered or AlreadyRegistered => $"Реєстрація",
            Reset => $"Тиждень {GetWeekNumber()}",
            Xur => $"Зур привіз свіжий крам",
            Bip => $"біп…",
            Modes => $"Режими",
            _ => string.Empty
        };
    }
}
