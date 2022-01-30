using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot.BotCommands
{
    internal interface ISlashCommand
    {
        string CommandName { get; }

        SlashCommandBuilder SlashCommand { get; }

        Task ExecuteCommandHelpAsync(SocketSlashCommand command);

        Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory);
    }
}
