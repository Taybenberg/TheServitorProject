using ActivityService;
using Discord;
using Discord.WebSocket;
using System.Globalization;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task ActivitySlashCommandExecutedAsync(SocketSlashCommand command)
        {
            var container = TryParseActivityContainer(command);

            if (container is not null)
            {
                var builder = new EmbedBuilder()
                    .WithColor(new Color(0xA6F167))
                    .WithTitle("Збір у активність")
                    .WithDescription($"Створюю активність…");

                await command.RespondAsync(embed: builder.Build(), ephemeral: true);

                await InitActivityAsync(container);
            }
            else
            {
                var builder = new EmbedBuilder()
                    .WithColor(new Color(0xD50000))
                    .WithTitle("Збір у активність")
                    .WithDescription($"Сталася помилка під час створення активності. Перевірте, чи формат команди коректний.\n" +
                        $"Щоби переглянути довідку, скористайтеся командою **допомога**.");

                await command.RespondAsync(embed: builder.Build(), ephemeral: true);
            }
        }
    }
}
