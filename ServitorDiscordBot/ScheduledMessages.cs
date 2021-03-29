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
        private async Task Bumper_Notify(Dictionary<ulong, (string, DateTime)> users)
        {
            _logger.LogInformation($"{DateTime.Now} Bump notification");

            IMessageChannel channel = _client.GetChannel(bumpChannelId) as IMessageChannel;

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessageColors.BumpNotification);

            builder.Description = "Саме час **!bump**-нути :alarm_clock:";

            if (users.Count > 0)
            {
                builder.Description += "\nКулдаун до:";

                builder.Fields = new();

                foreach (var user in users)
                    builder.Fields.Add(new EmbedFieldBuilder
                    {
                        Name = user.Value.Item1,
                        Value = user.Value.Item2.ToString("HH:mm:ss"),
                        IsInline = true
                    });
            }

            string mentions = string.Empty;

            foreach (var id in bumpPingUsers.Where(x => !users.ContainsKey(x)))
                mentions += $"<@{id}> ";

            await channel.SendMessageAsync(mentions, embed: builder.Build());
        }

        public async Task EververseNotificationAsync(SocketMessage message = null, string week = null)
        {
            int currWeek = 0;
            int.TryParse(week, out currWeek);

            if (currWeek < 1 || currWeek > 13)
                currWeek = (int)(DateTime.Now - seasonStart).TotalDays / 7 + 1;

            using var parser = new EververseParser();
            using var inventory = await parser.GetEververseInventoryAsync(seasonName, seasonStart, currWeek);

            IMessageChannel channel;

            if (message is null)
            {
                _logger.LogInformation($"{DateTime.Now} Eververse update");

                channel = _client.GetChannel(channelId) as IMessageChannel;

                var builder = new EmbedBuilder();

                builder.Color = GetColor(MessageColors.Eververse);

                builder.Title = $"Еверверс оновила асортимент";

                builder.Footer = GetFooter();

                await channel.SendMessageAsync(embed: builder.Build());
            }
            else
                channel = message.Channel;

            await channel.SendFileAsync(inventory, "EververseInventory.png");
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

                builder.Color = GetColor(MessageColors.Xur);

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
