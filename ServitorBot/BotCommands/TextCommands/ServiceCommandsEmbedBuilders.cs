using Discord;

namespace ServitorBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private EmbedBuilder DonateEmbedBuilder =>
            new EmbedBuilder()
                .WithColor(0xFBC02D)
                .WithThumbnailUrl(_client.GetUser(228896926991515649).GetAvatarUrl())
                .WithTitle("Підтримати автора")
                .WithDescription($"Ви завжди можете підтримати <@228896926991515649> кавою на сервісі [Buy Me a Coffee](https://www.buymeacoffee.com/servitor).");

        private EmbedBuilder NoPermissonForDeletionEmbedBuilder =>
            new EmbedBuilder()
                .WithColor(0xD50000)
                .WithDescription("У вас відсутні права на видалення повідомлень.");
    }
}
