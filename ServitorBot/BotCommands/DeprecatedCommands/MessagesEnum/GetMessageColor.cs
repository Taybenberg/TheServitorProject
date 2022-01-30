using Discord;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private Color GetColor(MessagesEnum messagesEnum) => messagesEnum switch
        {
            Error => new Color(0xd50000),
            BumpNotification => new Color(0x7488ff),
            MyGrandmasters => new Color(0xd81b60),
            MyRaids => new Color(0xa4c9fc),
            MyPartners => new Color(0xb4a647),
            AlreadyRegistered => new Color(0xffdd71),
            Register => new Color(0xddfab0),
            RegisterSuccessful => new Color(0xa6f167),
            RegisterNeedMoreInfo => new Color(0xff8c67),
            Suspicious => new Color(0x7fa2b2),
            Xur => new Color(0xe0f7fa),
            Reset => new Color(0xd5e1f6),
            ClanStats => new Color(0x8Be18a),
            Leaderboard => new Color(0x25c486),
            Raid => new Color(0x7c4dff),
            _ => new Color(0xfafafa)
        };
    }
}
