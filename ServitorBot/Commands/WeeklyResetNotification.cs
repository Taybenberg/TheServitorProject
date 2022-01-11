using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task WeeklyResetNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Weekly reset");

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            await GetWeeklyResetAsync(channel);
        }
    }
}
