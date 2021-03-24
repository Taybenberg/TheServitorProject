using BungieNetApi;
using Discord;
using Discord.WebSocket;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Bumper_Notify(IEnumerable<KeyValuePair<string, DateTime>> users)
        {
            _logger.LogInformation($"{DateTime.Now} Bump notification");

            IMessageChannel channel = _client.GetChannel(bumpChannelId) as IMessageChannel;

            var builder = new EmbedBuilder();

            builder.Color = (Color?)System.Drawing.ColorTranslator.FromHtml("#ADC8D1");

            builder.Description = "Саме час **!bump**-нути :alarm_clock:";

            if (users.Count() > 0)
            {
                builder.Description += "\nКулдаун до:";

                builder.Fields = new();

                foreach (var user in users)
                    builder.Fields.Add(new EmbedFieldBuilder
                    {
                        Name = user.Key,
                        Value = user.Value.ToString("HH:mm:ss"),
                        IsInline = true
                    });
            }

            await channel.SendMessageAsync(embed: builder.Build());
        }

        public async Task XurNotificationAsync(SocketMessage message = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            using var inventory = await apiCient.GetXurInventoryAsync(message is not null);

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

            await channel.SendFileAsync(inventory, "XurInventory.png");
        }
    }
}
