using Discord;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        public async Task GetWeeklyMilestoneAsync(IMessageChannel channel)
        {
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
        }
    }
}
