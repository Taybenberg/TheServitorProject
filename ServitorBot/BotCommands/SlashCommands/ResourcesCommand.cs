using DestinyInfocardsService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class ResourcesCommand : ISlashCommand
    {
        public string CommandName => "ресурси";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути асортимент Павука, Ади та Банші");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда генерує інформаційну картку з відомостями про асортимент ресурсів та модів " +
                    $"Ади-1, Банші-44 та Павука поточного денного ресету.\n" +
                    $"Картка надсилається автоматично після кожного денного ресету.\n" +
                    $"Інформація підтягується з ресурсу https://www.todayindestiny.com/vendors");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            using var scope = scopeFactory.CreateScope();

            var destinyInfocards = scope.ServiceProvider.GetRequiredService<IDestinyInfocards>();
            /*
            var infocard = await destinyInfocards.GetEververseInfocardAsync();

            var builder = InfocardHelper.ParseInfocard(infocard);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
            */
        }
    }
}
