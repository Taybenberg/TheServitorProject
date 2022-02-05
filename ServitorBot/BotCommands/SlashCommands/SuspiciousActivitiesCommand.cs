using ClanActivitiesService;
using CommonData.Localization;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class SuspiciousActivitiesCommand : ISlashCommand
    {
        public string CommandName => "відступники";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Виявити останні потенційно небезпечні активності")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("активність")
                    .WithDescription("Тип активності")
                    .WithRequired(false)
                    .AddSuspiciousActivitiesChoises());

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда виводить список останніх активностей, які не мають підбору гравців " +
                            $"та у яких були учасники клану з користувачами, які не є учасниками клану.\n" +
                            $"Список активностей містить список користувачів, які були в даній активності та їхні клантеги.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            var option = command.Data.Options.FirstOrDefault();

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var suspiciousActivities = option is null ?
                await clanActivities.GetSuspiciousActivitiesAsync() :
                await clanActivities.GetSuspiciousActivitiesAsync(Translation.GetActivityType(((string)option.Value).ToLower()));

            var sb = new StringBuilder(1950);
            foreach (var a in suspiciousActivities)
            {
                var title = $"**{a.Period}, {Translation.ActivityNames[a.ActivityType][0]} {a.Score}**";

                var users = string.Join('\n', a.Users
                    .Select(y => y.IsClanMember ?
                    $"**{y.UserName} [{y.ClanSign}]**" :
                    $"{y.UserName} [{y.ClanSign}] {y.ClanName}"));

                var str = $"{title}\n{users}\n\n";

                if (sb.Length + str.Length > 1950)
                    break;

                sb.Append(str);
            }

            var builder = new EmbedBuilder()
                .WithColor(0x7FA2B2)
                .WithTitle("Останні активності")
                .WithDescription($"||{sb.ToString()}||");

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
