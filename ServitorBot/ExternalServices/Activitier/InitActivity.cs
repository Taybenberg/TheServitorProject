﻿using ActivityService;
using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
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