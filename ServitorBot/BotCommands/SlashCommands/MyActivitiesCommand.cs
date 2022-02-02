using ClanActivitiesService;
using CommonData.Localization;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class MyActivitiesCommand : ISlashCommand
    {
        public string CommandName => "мої_активності";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Кількість ваших активностей за весь час")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("період")
                    .WithDescription("Кількість ваших активностей за вказаний період")
                    .WithRequired(false)
                    .AddPeriodChoises());

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда обчислює кількість ваших активностей, які ви закрили перебуваючи учасником клану за вказаний період " +
                            $"або за весь час (але не раніше 01.01.2021).\n" +
                            $"Підрахунок ведеться загалом на акаунт, на окремих персонажів та для кожного типу активності окремо.\n" +
                            $"Додатково виводиться інфографіка, яка показує % активностей " +
                            $"ПвП (горнило), ПвПвЕ (гамбіт) та ПвЕ (все інше).\n" +
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
                await clanActivities.GetUserActivitiesAsync(command.User.Id) :
                await clanActivities.GetUserActivitiesAsync(command.User.Id, CommandHelper.GetPeriod((string)option.Value));

            if (userContainer is null)
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.UserIsNotRegisteredBuilder.Build());
                return;
            }

            var sb = new StringBuilder($"{CommandHelper.GetActivityCountImpression(userContainer.TotalCount, command.User.Mention)}");

            sb.Append("\n\n***За класами:***");
            foreach (var character in userContainer.ClassCounters)
            {
                var emoji = CommonData.DiscordEmoji.Emoji.GetClassEmoji(character.Class);
                var className = Translation.ClassNames[character.Class];

                sb.Append($"\n{emoji} **{className}** – **{character.Count}**");
            }

            sb.Append("\n\n***За типом активности:***");
            foreach (var mode in userContainer.ModeCounters.Counters)
            {
                var emoji = CommonData.DiscordEmoji.Emoji.GetActivityEmoji(mode.ActivityMode);
                var modes = Translation.ActivityNames[mode.ActivityMode];

                sb.Append($"\n{emoji} **{modes[0]}** | {modes[1]} – **{mode.Count}**");
            }

            var builder = new EmbedBuilder()
                .WithColor(0xFACC3F)
                .WithThumbnailUrl(command.User.GetAvatarUrl())
                .WithTitle($"Активності {userContainer.UserName}{(option is null ? string.Empty : $" за {option.Value}")}")
                .WithImageUrl(userContainer.ChartImageURL)
                .WithDescription(sb.ToString());

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
