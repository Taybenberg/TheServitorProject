using DestinyInfocardsService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class EververseCommand : ISlashCommand
    {
        public string CommandName => "еверверс";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути асортимент Тесс Еверіс")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("тиждень")
                    .WithDescription("Номер тижня, за який бажаєте переглянути асортимент")
                    .WithRequired(false)
                    .WithType(ApplicationCommandOptionType.Integer));

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда генерує інформаційну картку з відомостями про асортимент Тесс Еверіс за визначений тиждень.\n" +
                    $"Якщо вказано параметр, формат якого не відповідає вказаним вище (або номер тижня виходить за діапазон сезону), " +
                    $"то буде виведено асортимент поточного тижня.\n" +
                    $"Картка надсилається автоматично після кожного тижневого ресету.\n" +
                    $"Можливе виведення хибної інформації на початку сезону, допоки не сформовано таблицю сезонного лутпулу.\n" +
                    $"Інформація підтягується з ресурсу https://www.todayindestiny.com/eververseCalendar");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            var option = command.Data.Options.FirstOrDefault();

            using var scope = scopeFactory.CreateScope();

            var destinyInfocards = scope.ServiceProvider.GetRequiredService<IDestinyInfocards>();

            var infocard = option is null ?
                await destinyInfocards.GetEververseInfocardAsync() :
                await destinyInfocards.GetEververseInfocardAsync((int)(long)option.Value);

            var builder = InfocardHelper.ParseInfocard(infocard);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
