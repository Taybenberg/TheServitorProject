using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetClanActivitiesAsync(IMessage message, string period)
        {
            (var date, var title) = GetPeriod(period);

            var builder = GetBuilder(MessagesEnum.ClanActivities, message);

            builder.Title += title;

            var counter = await getWrapperFactory().GetClanActivitiesAsync(date);

            builder.ThumbnailUrl = (message.Channel as IGuildChannel).Guild.IconUrl;

            builder.Description = $"{GetActivityCountImpression(counter.Count, "клану")}\n\n***По типу активності:***";

            foreach (var count in counter.Modes)
                builder.Description += $"\n{count.Emoji} **{count.Modes[0]}** | {count.Modes[1]} – **{count.Count}**";

            builder.ImageUrl = counter.QuickChartURL;

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
