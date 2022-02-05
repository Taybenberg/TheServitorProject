using DestinyNotificationsService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class LostSectorsInfocardCommand : ISlashCommand
    {
        public string CommandName => "сектори";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути пул сьогоднішніх загублених секторів");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда генерує інформаційну картку з відомостями про загублені сектори " +
                            $"складності \"легенда\" та \"майстер\" поточного денного ресету.\n" +
                            $"Картка надсилається автоматично після кожного денного ресету.\n" +
                            $"На початку нового сезону або з появою нових секторів можливе виведення хибної інформації.\n" +
                            $"Інформація підтягується з ресурсу https://www.todayindestiny.com/");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            using var scope = scopeFactory.CreateScope();

            var notifications = scope.ServiceProvider.GetRequiredService<IDestinyNotifications>();

            var infocard = await notifications.GetLostSectorsInfocardAsync();

            var builder = InfocardHelper.ParseInfocard(infocard);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
