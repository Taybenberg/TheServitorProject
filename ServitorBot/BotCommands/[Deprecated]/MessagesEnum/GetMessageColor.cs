using Discord;
using static ServitorBot.MessagesEnum;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private Color GetColor(MessagesEnum messagesEnum) => messagesEnum switch
        {
            Error => new Color(0xd50000),
            BumpNotification => new Color(0x7488ff),
            MyGrandmasters => new Color(0xd81b60),
            MyRaids => new Color(0xa4c9fc),
            Reset => new Color(0xd5e1f6),
            Raid => new Color(0x7c4dff),
            _ => new Color(0xfafafa)
        };
    }
}
