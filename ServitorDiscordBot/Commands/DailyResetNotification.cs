using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task DailyResetNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Daily reset");

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            await GetDailyResetAsync(channel);

            await GetRoadmapAsync(channel);
        }
    }
}
