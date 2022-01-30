using CommonData;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ClanActivitiesService;
using System.Text;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    public class ClanActivitiesCommand : ISlashCommand
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
                    .WithType(ApplicationCommandOptionType.String)
                    .AddChoice("останній тиждень", "останній тиждень")
                    .AddChoice("останній місяць", "останній місяць")
                    .AddChoice("останній рік", "останній рік"));

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
            await CommandHelper.SendWaitResponceAsync(command);

            var option = command.Data.Options.FirstOrDefault();

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var modeContainer = option is null ?
                await clanActivities.GetClanActivitiesAsync() :
                await clanActivities.GetClanActivitiesAsync(GetPeriod((string)option.Value));

            var sb = new StringBuilder();

            foreach (var mode in modeContainer.Counters)
            {
                var emoji = CommonData.DiscordEmoji.Emoji.GetActivityEmoji(mode.ActivityMode);
                var modes = CommonData.Localization.Translation.ActivityNames[mode.ActivityMode];

                sb.Append($"\n{emoji} **{modes[0]}** | {modes[1]} – **{mode.Count}**");
            }

            var builder = new EmbedBuilder()
                .WithColor(0xFFB95A)
                .WithThumbnailUrl((command.Channel as IGuildChannel).Guild.IconUrl)
                .WithTitle($"Активності клану {(command.Channel as IGuildChannel).Guild.Name}{(option is null ? string.Empty : $" за {option.Value}")}")
                .WithImageUrl(modeContainer.ChartImageURL)
                .WithDescription($"{CommandHelper.GetActivityCountImpression(modeContainer.TotalCount, "клану")}\n\n***По типу активності:***{sb.ToString()}");

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }

        private DateTime? GetPeriod(string value) =>
            value switch
            {
                "останній тиждень" => DateTime.UtcNow.AddDays(-7),
                "останній місяць" => DateTime.UtcNow.AddMonths(-1),
                "останній рік" => DateTime.UtcNow.AddYears(-1),
                _ => null
            };
    }
}
