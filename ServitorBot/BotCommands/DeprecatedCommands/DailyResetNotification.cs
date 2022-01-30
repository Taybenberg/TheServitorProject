using Discord;
using Microsoft.Extensions.Logging;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task DailyResetNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Daily reset");

            var channel = _client.GetChannel(_mainChannelIDs[0]) as IMessageChannel;

            //await GetDailyResetAsync(channel);
        }
    }
}
