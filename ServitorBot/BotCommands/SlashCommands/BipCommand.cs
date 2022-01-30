using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class BipCommand : ISlashCommand
    {
        public string CommandName => "біп";

        public SlashCommandBuilder SlashCommand => 
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Перевірити працездатність бота");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда допомагає визначити, чи функціонує у цей момент бот.\n" +
                            $"Якщо бот функціонує, то у відповідь ви отримаєте повідомлення **буп…**\n" +
                            $"Використовуйте цю команду, якщо ви не отримали результат іншої команди, або якщо вважаєте, що бот може не працювати.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xAE52D4)
                .WithTitle("буп…");

            await command.RespondAsync(embed: builder.Build());
        }
    }
}
