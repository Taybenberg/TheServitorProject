using ClanActivitiesService;
using CommonData.Localization;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class ClanActivitiesCommand : ISlashCommand
    {
        public string CommandName => "кланові_активності";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Кількість активностей клану за весь час")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("період")
                    .WithDescription("Кількість активностей клану за вказаний період")
                    .WithRequired(false)
                    .AddPeriodChoises());

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда обчислює кількість активностей, які закрили учасники клану за вказаний період " +
                            $"або за весь час (але не раніше 01.01.2021).\n" +
                            $"Підрахунок ведеться загалом та для кожного типу активності окремо.\n" +
                            $"Додатково виводиться інфографіка, яка показує % активностей " +
                            $"ПвП (горнило), ПвПвЕ (гамбіт) та ПвЕ (все інше).");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            var option = command.Data.Options.FirstOrDefault();

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var modeContainer = option is null ?
                await clanActivities.GetClanActivitiesAsync() :
                await clanActivities.GetClanActivitiesAsync(CommandHelper.GetPeriod((string)option.Value));

            var sb = new StringBuilder($"{CommandHelper.GetActivityCountImpression(modeContainer.TotalCount, "клану")}");

            sb.Append("\n\n***За типом активности:***");
            foreach (var mode in modeContainer.Counters)
            {
                var emoji = CommonData.DiscordEmoji.Emoji.GetActivityEmoji(mode.ActivityMode);
                var modes = Translation.ActivityNames[mode.ActivityMode];

                sb.Append($"\n{emoji} **{modes[0]}** | {modes[1]} – **{mode.Count}**");
            }

            var builder = new EmbedBuilder()
                .WithColor(0xFFB95A)
                .WithThumbnailUrl((command.Channel as IGuildChannel).Guild.IconUrl)
                .WithTitle($"Активності клану {(command.Channel as IGuildChannel).Guild.Name}{(option is null ? string.Empty : $" за {option.Value}")}")
                .WithImageUrl(modeContainer.ChartImageURL)
                .WithDescription(sb.ToString());

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
