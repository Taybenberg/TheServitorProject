using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ServitorDiscordBot.BotCommands.SlashCommands;

namespace ServitorDiscordBot.BotCommands
{
    internal interface ISlashCommand
    {
        string CommandName { get; }

        SlashCommandBuilder SlashCommand { get; }

        Task ExecuteCommandHelpAsync(SocketSlashCommand command);

        Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory);

        static ISlashCommand[] SlashCommands =>
            new ISlashCommand[]
            {
                new HelpCommand(),
                new BipCommand(),
            };

        static async Task SendWaitResponceAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithTitle(CommonData.DiscordEmoji.Emoji.Loading)
                .WithDescription("Зачекайте, команда виконується…");

            await command.RespondAsync(embed: builder.Build());
        }
    }
}
