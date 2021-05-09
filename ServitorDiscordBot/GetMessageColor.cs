using Discord;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        Color GetColor(MessagesEnum messagesEnum) => messagesEnum switch
        {
            Wait => new Color(0x616161),
            Error => new Color(0xd50000),
            BumpNotification => new Color(0x3949ab),
            Bumped => new Color(0x6f74dd),
            Bip => new Color(0xae52d4),
            Help => new Color(0x7c43bd),
            Modes => new Color(0x63a4ff),
            ClanActivities => new Color(0xe4e65e),
            MyActivities => new Color(0xffca28),
            MyPartners => new Color(0x82e9de),
            NotRegistered => new Color(0xfb8c00),
            AlreadyRegistered => new Color(0xffdd71),
            Register => new Color(0x81d4fa),
            RegisterSuccessful => new Color(0x76ff03),
            RegisterNeedMoreInfo => new Color(0xff7043),
            Suspicious => new Color(0x546e7a),
            Xur => new Color(0xe0f7fa),
            Reset => new Color(0xffffcf),
            ClanStats => new Color(0x76d275),
            Leaderboard => new Color(0x2962ff),
            _ => new Color(0xfafafa)
        };
    }
}
