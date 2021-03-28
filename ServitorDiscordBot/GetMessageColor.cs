using System.Drawing;
using static ServitorDiscordBot.MessageColors;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        Discord.Color? GetColor(MessageColors colors)
        {
            Color? color = colors switch
            {
                Error => ColorTranslator.FromHtml("#d50000"),
                BumpNotification => ColorTranslator.FromHtml("#3949ab"),
                Bumped => ColorTranslator.FromHtml("#6f74dd"),
                Bip => ColorTranslator.FromHtml("#ae52d4"),
                Help => ColorTranslator.FromHtml("#7c43bd"),
                Modes => ColorTranslator.FromHtml("#63a4ff"),
                ClanActivities => ColorTranslator.FromHtml("#e4e65e"),
                MyActivities => ColorTranslator.FromHtml("#ffca28"),
                MyPartners => ColorTranslator.FromHtml("#82e9de"),
                NotRegistered => ColorTranslator.FromHtml("#fb8c00"),
                AlreadyRegistered => ColorTranslator.FromHtml("#ffdd71"),
                Register => ColorTranslator.FromHtml("#81d4fa"),
                RegisterSuccessful => ColorTranslator.FromHtml("#76ff03"),
                RegisterNeedMoreInfo => ColorTranslator.FromHtml("#ff7043"),
                Suspicious => ColorTranslator.FromHtml("#546e7a"),
                Xur => ColorTranslator.FromHtml("#e0f7fa"),
                Eververse => ColorTranslator.FromHtml("#ffffcf"),
                ClanStats => ColorTranslator.FromHtml("#76d275"),
                Leaderboard => ColorTranslator.FromHtml("#2962ff"),
                _ => ColorTranslator.FromHtml("#fafafa")
            };

            return (Discord.Color?)color;
        }
    }
}
