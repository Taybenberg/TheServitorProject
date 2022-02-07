using ClanActivitiesService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class MyRaidsCommand : ISlashCommand
    {
        public string CommandName => "мої_рейди";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути закриті вами рейди цього тижня");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда виводить список закритих вами рейдів цього тижня на різних персонажах.\n" +
                            $"Враховуються лише ті рейди, які ви закрили перебуваючи учасником клану.\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var builder = new EmbedBuilder()
                .WithColor(0xA4C9FC)
                .WithThumbnailUrl(command.User.GetAvatarUrl())
                .WithTitle($"Рейди {command.User.Username}");

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
/*
            var raids = await getWrapperFactory().GetMyRaidsAsync(message.Author.Id, _seasonStart, GetWeekNumber());

            if (!raids.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyRaids, message, userName: raids.UserName);

            builder.ThumbnailUrl = message.Author.GetAvatarUrl();

            builder.Description = "Закриття за поточний тиждень:";

            builder.Fields = raids.Classes.Select(x => new EmbedFieldBuilder
            {
                Name = $"{x.Emoji} {x.Class}",
                Value = x.Raids.Any() ? string.Join("\n", x.Raids.Select(r => $"{r.Emoji} {r.Name}")) : "Немає",
                IsInline = false
            }).ToList();

            await message.Channel.SendMessageAsync(embed: builder.Build());
            */