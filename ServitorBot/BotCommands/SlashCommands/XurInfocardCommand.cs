using DestinyInfocardsService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class XurInfocardCommand : ISlashCommand
    {
        public string CommandName => "зур";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути асортимент Зура");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда генерує інформаційну картку з відомостями про поточний асортимент Зура.\n" +
                            $"Буде згенеровано порожню картку, якщо на момент виклику команди Зур відсутній, або сервери Destiny не працюють.\n" +
                            $"Картка надсилається автоматично щоп'ятниці після денного ресету.\n" +
                            $"Місцеперебування Зура підтягується з ресурсу https://xur.wiki/");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            using var scope = scopeFactory.CreateScope();

            var destinyInfocards = scope.ServiceProvider.GetRequiredService<IDestinyInfocards>();

            var infocard = await destinyInfocards.GetXurInfocardAsync();

            var builder = InfocardHelper.ParseInfocard(infocard);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
