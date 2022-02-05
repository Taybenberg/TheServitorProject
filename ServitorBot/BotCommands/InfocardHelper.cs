using DestinyNotificationsService.Infocards;
using Discord;

namespace ServitorDiscordBot.BotCommands
{
    internal static class InfocardHelper
    {
        public static EmbedBuilder ParseInfocard(XurInfocard infocard) =>
            new EmbedBuilder()
                .WithColor(0xE0F7FA);

        public static EmbedBuilder ParseInfocard(LostSectorsInfocard infocard) =>
            new EmbedBuilder()
                .WithColor(0xE0F7FA);

        public static EmbedBuilder ParseInfocard(EververseInfocard infocard) =>
            new EmbedBuilder()
                .WithColor(0xE0F7FA);
    }
}
