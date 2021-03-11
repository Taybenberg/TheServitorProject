using BungieNetApi;
using Discord;
using Discord.WebSocket;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task XurNotificationAsync(SocketMessage message = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            using var inventory = await apiCient.GetXurInventoryAsync();

            IMessageChannel channel;

            if (message is null)
            {
                _logger.LogInformation($"{DateTime.Now} Xur arrived");

                channel = _client.GetChannel(channelId) as IMessageChannel;

                var builder = new EmbedBuilder();

                builder.Color = (Color?)System.Drawing.ColorTranslator.FromHtml("#ADC8D1");

                builder.Title = $"Зур привіз свіжий крам";

                builder.Footer = GetFooter();

                await channel.SendMessageAsync(embed: builder.Build());
            }
            else
                channel = message.Channel;

            await channel.SendFileAsync(inventory, "Inventory.png");
        }
    }
}
