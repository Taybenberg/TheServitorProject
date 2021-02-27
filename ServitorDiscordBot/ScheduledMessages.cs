using System;
using System.Web;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task XurNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Xur arrived");

            var channel = _client.GetChannel(channelId) as IMessageChannel;

            await channel.SendMessageAsync("");
        }
    }
}
