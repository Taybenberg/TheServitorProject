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

            var builder = GetBuilder(MessagesEnum.Reset, null);

            builder.Description = "Відбувся денний ресет";

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendMessageAsync(embed: builder.Build());

            await GetLostSectorsLootAsync(channel);

            await GetResourcesPoolAsync(channel);

            await GetRoadmapAsync(channel);
        }
    }
}
