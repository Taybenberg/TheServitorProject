using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesService;
using CommonData.Localization;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class ClanStatsCommand : ISlashCommand
    {
        public string CommandName => "статистика_клану";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Агрегована статистика клану у режимі …")
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
                .WithDescription($"Команда виводить агреговану статистику учасників вашого клану за різними показниками у вказаному типі активності.\n" +
                            $"Переглянути список типів активностей можна за допомогою команди **режими**.\n" +
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

            var clanStats = await clanActivities.GetClanStatsAsync(command.User.Id, mode);

            if (clanStats is null)
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.UserIsNotRegisteredBuilder.Build());
                return;
            }

            if (!clanStats.Any())
            {
                await command.ModifyOriginalResponseAsync(x => x.Embed = CommandHelper.BungieSideErrorBuilder.Build());
                return;
            }

            var fields = clanStats
                .Select(x =>
                   new EmbedFieldBuilder
                   {
                       Name = Translation.StatNames[x.StatName],
                       Value = x.Value,
                       IsInline = false
                   })
                .OrderBy(x => x.Name).ToList();

            var builder = new EmbedBuilder()
                .WithColor(0x8BE18A)
                .WithThumbnailUrl(Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityEmoji(mode)).Url)
                .WithTitle($"БЕТА | Статистика клану {(command.Channel as IGuildChannel).Guild.Name} | {Translation.ActivityNames[mode][0]}")
                .WithFields(fields);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
