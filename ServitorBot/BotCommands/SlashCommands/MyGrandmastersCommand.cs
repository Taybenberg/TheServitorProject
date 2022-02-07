using ClanActivitiesService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class MyGrandmastersCommand : ISlashCommand
    {
        public string CommandName => "мої_грандмайстри";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути закриті вами найтфоли складності \"Грандмайстер\"");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда виводить список закритих вами грандмайстрів у поточному сезоні та за весь час.\n" +
                            $"Враховуються лише ті грандмайстри, які ви закрили перебуваючи учасником клану (але не раніше 01.01.2021).\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var builder = new EmbedBuilder()
                .WithColor(0xD81B60)
                .WithThumbnailUrl(command.User.GetAvatarUrl())
                .WithTitle($"Грандмайстри {command.User.Username}");

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
/*
var grandmasters = await getWrapperFactory().GetMyGrandmastersAsync(message.Author.Id, _seasonStart);

if (!grandmasters.IsUserRegistered)
{
    await UserIsNotRegisteredAsync(message);

    return;
}

var builder = GetBuilder(MessagesEnum.MyGrandmasters, message, userName: grandmasters.UserName);

builder.ThumbnailUrl = message.Author.GetAvatarUrl();

builder.Fields = new List<EmbedFieldBuilder>
{
    new EmbedFieldBuilder
    {
        Name = $"Сезон {_seasonName}",
        Value = grandmasters.Seasonal.Any() ? string.Join("\n", grandmasters.Seasonal) : "Немає",
        IsInline = false
    },
    new EmbedFieldBuilder
    {
        Name = $"Весь час",
        Value = grandmasters.AllTime.Any() ? string.Join("\n", grandmasters.AllTime) : "Немає",
        IsInline = false
    }
};

await message.Channel.SendMessageAsync(embed: builder.Build());
*/