using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task XurNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Xur arrived");

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Xur, null, false);

            await channel.SendMessageAsync(embed: builder.Build());

            await GetXurInventoryAsync(channel, false);
        }
    }
}
