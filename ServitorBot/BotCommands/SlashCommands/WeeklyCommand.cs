using DestinyInfocardsService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class WeeklyCommand : ISlashCommand
    {
        public string CommandName => "тиждень";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Переглянути інформацію про поточний тижневий ресет");

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда дозволяє переглянути тижневу ротацію найтфолу та горнила, " +
                    $"а також визначає, чи доступний цього тижня Залізний стяг.\n" +
                    $"Картка надсилається автоматично після кожного тижневого ресету.\n" +
                    $"Якщо в момент виконання команди сервери Destiny не працюють, то результат команди не буде отримано.\n" +
                    $"Наявність Залізного стягу перевіряється на ресурсі https://www.light.gg");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            using var scope = scopeFactory.CreateScope();

            var destinyInfocards = scope.ServiceProvider.GetRequiredService<IDestinyInfocards>();
            /*
            var infocard = await destinyInfocards.GetEververseInfocardAsync();

            var builder = InfocardHelper.ParseInfocard(infocard);

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
            */
        }
    }
}
/*
var builder = GetBuilder(MessagesEnum.Reset, null);

var milestone = await getWrapperFactory().GetWeeklyMilestoneAsync();

if (milestone.IsIronBannerAvailable)
{
    builder.ThumbnailUrl = milestone.IronBannerImageURL;

    builder.Description = $"Доступний **{milestone.IronBannerName}**!";
}

builder.ImageUrl = milestone.NightfallImageURL;

builder.Fields = milestone.Fields.Select(x => new EmbedFieldBuilder
{
    Name = x.Name,
    Value = x.Value,
    IsInline = true
}).ToList();

await channel.SendMessageAsync(embed: builder.Build());
*/
