using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesService;
using CommonData.Localization;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class LeaderboardCommand : ISlashCommand
    {
        public string CommandName => "дошка_лідерів";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Список лідерів у режимі …")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("режим")
                    .WithDescription("Переглянути список режимів можна за допомогою команди \"режими\"")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.String));

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда виводить список лідерів вашого клану за різними показниками у вказаному типі активності.\n" +
                            $"Переглянути список типів активностей можна за допомогою команди **режими**.\n" +
                            $"Виводиться 3 користувачі з найкращим результатом певного показника, " +
                            $"якщо вас немає у першій трійці, то додатково вказується ваше місце у рейтингу.\n" +
                            $"Додатково виводиться інфографіка для ваших показників.\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.\n" +
                            $"Увага! Команда досі в бета-версії!");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            var option = command.Data.Options.FirstOrDefault();

            var mode = Translation.GetActivityType(((string)option.Value).ToLower());

            if (mode is DestinyActivityModeType.None)
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.WrongActivityTypeBuilder.Build());
                return;
            }

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var leaderboard = await clanActivities.GetLeaderboardAsync(command.User.Id, mode);

            if (leaderboard is null)
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.UserIsNotRegisteredBuilder.Build());
                return;
            }

            if (!leaderboard.LeaderboardStats.Any())
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.BungieSideErrorBuilder.Build());
                return;
            }

            var fields = leaderboard.LeaderboardStats
                .Select(x =>
                    new EmbedFieldBuilder
                    {
                        IsInline = false,
                        Name = x.StatName,
                        Value = string.Join("\n", x.Leaders
                            .Select(y => y.IsCurrUser ?
                                $"**{y.Rank}, {y.UserName}, {Translation.ClassNames[y.DestinyClass]}, {y.Value}**" :
                                $"{y.Rank}, {y.UserName}, {Translation.ClassNames[y.DestinyClass]}, {y.Value}"))
                    }).ToList();

            var builder = new EmbedBuilder()
                .WithColor(0x25C486)
                .WithThumbnailUrl(Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityEmoji(mode)).Url)
                .WithTitle($"БЕТА | Дошка лідерів | {Translation.ActivityNames[mode][0]}")
                .WithImageUrl(leaderboard.ChartImageURL)
                .WithFields(fields);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
