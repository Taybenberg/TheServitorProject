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

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            using var inventory = await apiCient.GetXurInventoryAsync();

            await channel.SendMessageAsync(embed: builder.Build());
            await channel.SendFileAsync(inventory, "Inventory.png");
        }
    }
}
