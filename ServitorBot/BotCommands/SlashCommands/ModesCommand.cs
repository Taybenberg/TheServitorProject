using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class ModesCommand : ISlashCommand
    {
        public string CommandName => "режими";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Cписок типів активностей");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда виводить список наявних типів активностей, " +
                            $"які можна використовувати у якості параметрів для інших команд.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            var builder = new EmbedBuilder()
               .WithColor(0xE4E65E)
               .WithTitle("Режими")
               .WithDescription(string.Join('\n', CommonData.Localization.Translation.StatsActivityNames
               .OrderBy(x => x.Value[0]).Select(x =>
               $"{CommonData.DiscordEmoji.Emoji.GetActivityEmoji(x.Key)} **{x.Value[0]}** | {x.Value[1]}")));

            await command.RespondAsync(embed: builder.Build());
        }
    }
}
