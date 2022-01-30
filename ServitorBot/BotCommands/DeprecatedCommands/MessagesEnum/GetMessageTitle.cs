using Discord;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private string GetTitle(MessagesEnum messagesEnum, IMessage message, string userName) => messagesEnum switch
        {
            Wait => CommonData.DiscordEmoji.Emoji.Loading,
            Leaderboard => $"БЕТА | Дошка лідерів",
            ClanStats => $"БЕТА | Статистика клану {(message.Channel as IGuildChannel).Guild.Name}",
            MyGrandmasters => $"Грандмайстри {userName}",
            MyRaids => $"Рейди {userName}",
            MyActivities => $"Активності {userName}",
            MyPartners => $"Побратими {userName}",
            ClanActivities => $"Активності клану {(message.Channel as IGuildChannel).Guild.Name}",
            Suspicious => $"Останні активності",
            Help => $"Допомога",
            Register or NotRegistered or AlreadyRegistered => $"Реєстрація",
            Reset => $"Тиждень {GetWeekNumber()}",
            Xur => $"Зур привіз свіжий крам",
            Bip => $"буп…",
            Modes => $"Режими",
            Raid => $"Рейд",
            _ => string.Empty
        };
    }
}
