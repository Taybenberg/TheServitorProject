using static ServitorBot.MessagesEnum;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private readonly static Dictionary<MessagesEnum, string[]> messageCommands = new()
        {
            [Weekly] = new string[] { "тиждень", "weekly" },
            [Sectors] = new string[] { "сектори", "sectors" },
            [Resources] = new string[] { "ресурси", "resources" },
            [MyGrandmasters] = new string[] { "мої грандмайстри", "my grandmasters" },
            [MyRaids] = new string[] { "мої рейди", "my raids" },
            [Eververse] = new string[] { "еверверс", "eververse" },
            [EververseAll] = new string[] { "еверверс все", "eververse all" },
        };
    }
}
