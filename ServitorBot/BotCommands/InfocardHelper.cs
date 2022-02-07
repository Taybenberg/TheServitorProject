using DestinyInfocardsService.Infocards;
using Discord;

namespace ServitorBot.BotCommands
{
    internal static class InfocardHelper
    {
        public static EmbedBuilder ParseInfocard(XurInfocard infocard) =>
            new EmbedBuilder()
                .WithColor(0xE0F7FA)
                .WithTitle("Зур")
                .WithDescription($"**Локація: {infocard.XurLocation ?? "Невизначено"}**")
                .WithImageUrl(infocard.InfocardImageURL);

        public static EmbedBuilder ParseInfocard(LostSectorsInfocard infocard) =>
            new EmbedBuilder()
                .WithColor(0xE0F7FA)
                .WithTitle($"Загублені сектори")
                .WithDescription($"**{infocard.ResetBegin.ToString("dd.MM.yyyy HH:mm")} - {infocard.ResetEnd.ToString("dd.MM.yyyy HH:mm")}**")
                .WithImageUrl(infocard.InfocardImageURL);

        public static EmbedBuilder ParseInfocard(EververseInfocard infocard) =>
            new EmbedBuilder()
                .WithColor(0xE0F7FA);
    }
}
