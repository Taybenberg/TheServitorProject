using ActivityService;
using Discord;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task InitActivityAsync(ActivityContainer container)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xFFFFFF)
                .WithTitle(CommonData.DiscordEmoji.Emoji.Loading)
                .WithDescription("Зачекайте, ініціалізую збір…");

            IMessageChannel channel = _client.GetChannel(container.ChannelID) as IMessageChannel;

            var message = await channel.SendMessageAsync(embed: builder.Build());

            container.ActivityID = message.Id;
            container.PlannedDate = container.PlannedDate.ToUniversalTime();

            await _activityManager.AddActivityAsync(container);
        }
    }
}