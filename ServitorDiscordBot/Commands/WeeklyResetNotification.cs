using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task WeeklyResetNotificationAsync(IMessageChannel channel = null)
        {
            var builder = GetBuilder(MessagesEnum.Reset, null);

            var milestone = await getStatsFactory().GetWeeklyMilestoneAsync();

            string additionalDescription = string.Empty;

            if (milestone.IsIronBannerAvailable)
            {
                builder.ThumbnailUrl = milestone.IronBannerImageURL;

                additionalDescription += $"Доступний **{milestone.IronBannerName}**!";
            }

            builder.ImageUrl = milestone.NightfallImageURL;

            builder.Fields = milestone.Fields.Select(x => new EmbedFieldBuilder
            {
                Name = x.Name,
                Value = x.Value,
                IsInline = true
            }).ToList();

            if (channel is null)
            {
                _logger.LogInformation($"{DateTime.Now} Weekly reset");

                builder.Description = $"Відбувся тижневий ресет\n{additionalDescription}";

                channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

                await channel.SendMessageAsync(embed: builder.Build());

                await GetEververseInventoryAsync(channel);
            }
            else
            {
                builder.Description = additionalDescription;

                await channel.SendMessageAsync(embed: builder.Build());
            }
        }
    }
}
