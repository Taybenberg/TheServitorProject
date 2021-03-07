using BungieNetApi;
using Discord;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
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

            var channel = _client.GetChannel(channelId) as IMessageChannel;

            var builder = new EmbedBuilder();

            builder.Color = Color.DarkPurple;

            builder.Title = $"Зур привіз свіжий крам";

            builder.Footer = GetFooter();

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            await channel.SendFileAsync(await apiClient.GetXurInventoryAsync(), "Inventory.png", embed: builder.Build());
        }
    }
}
