using ClanActivitiesService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class MyPartnersCommand : ISlashCommand
    {
        public string CommandName => "мої_побратими";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Список ваших побратимів за весь час")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("період")
                    .WithDescription("Список ваших побратимів за вказаний період")
                    .WithRequired(false)
                    .AddPeriodChoises());

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда обчислює кількість активностей, які ви закрили перебуваючи учасником клану " +
                            $"за вказаний період або за весь час (але не раніше 01.01.2021) разом з іншими учасниками клану (кооперативні активності).\n" +
                            $"Також обчислюється співвідношення кооперативних активностей і загальної кількості активностей.\n" +
                            $"Список виводиться за спаданням кількості спільних активностей.\n" +
                            $"Для десяти найбільш споріднених гравців з вами додатково виводиться інфографіка, " +
                            $"яка показує кількість активностей ПвП (горнило), ПвПвЕ (гамбіт) та ПвЕ (все інше).\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            var option = command.Data.Options.FirstOrDefault();

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var userContainer = option is null ?
                await clanActivities.GetUserPartnersAsync(command.User.Id) :
                await clanActivities.GetUserPartnersAsync(command.User.Id, CommandHelper.GetPeriod((string)option.Value));

            if (userContainer is null)
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.UserIsNotRegisteredBuilder.Build());
                return;
            }

            var sb = new StringBuilder($"Всього кооперативних активностей: ");
            sb.Append($"**{userContainer.CoopCount}/{userContainer.TotalCount} ({Math.Round(userContainer.CoopCount * 100.0 / userContainer.TotalCount, 2)}%)**\n");

            foreach (var partner in userContainer.Partners)
                sb.Append($"\n**{partner.UserName}** – **{partner.Count}**");

            var builder = new EmbedBuilder()
                .WithColor(0xB4A647)
                .WithThumbnailUrl(command.User.GetAvatarUrl())
                .WithTitle($"Побратими {userContainer.UserName}{(option is null ? string.Empty : $" за {option.Value}")}")
                .WithImageUrl(userContainer.ChartImageURL)
                .WithDescription(sb.ToString());

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
