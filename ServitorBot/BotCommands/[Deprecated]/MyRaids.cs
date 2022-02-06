using Discord;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task GetMyRaidsAsync(IMessage message)
        {
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
        }
    }
}
