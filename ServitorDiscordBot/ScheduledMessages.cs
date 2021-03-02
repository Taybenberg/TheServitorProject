using System;
using System.Web;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.WebSocket;
using Extensions;

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

            var xur = await Xur.GetXurAsync();

            builder.Title = $"Зур привіз крам на {xur.LocationName}";
            builder.ThumbnailUrl = xur.LocationIcon;

            builder.Fields = new();
            foreach (var item in xur.Items)
                builder.Fields.Add(new EmbedFieldBuilder{ Name = item.ItemName, Value = "WIP", IsInline = false});
            
            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
