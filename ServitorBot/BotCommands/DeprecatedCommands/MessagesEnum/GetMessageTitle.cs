using Discord;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private string GetTitle(MessagesEnum messagesEnum, IMessage message, string userName) => messagesEnum switch
        {
            MyGrandmasters => $"Грандмайстри {userName}",
            MyRaids => $"Рейди {userName}",
            Reset => $"Тиждень {GetWeekNumber()}",
            Xur => $"Зур привіз свіжий крам",
            Raid => $"Рейд",
            _ => string.Empty
        };
    }
}
