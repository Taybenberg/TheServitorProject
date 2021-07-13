using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Bumper_Notify(Dictionary<string, DateTime> users)
        {
            _logger.LogInformation($"{DateTime.Now} Bump notification");

            IMessageChannel channel = _client.GetChannel(_bumpChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.BumpNotification, null);

            builder.Description = "Саме час **!bump**-нути :alarm_clock:";

            if (users.Count > 0)
            {
                builder.Description += "\nКулдаун до:";

                foreach (var user in users)
                    builder.Description += $"\n<@{user.Key}> – *{user.Value.ToString("HH:mm:ss")}*";
            }

            string mentions = string.Empty;

            foreach (var id in _bumpPingUsers.Where(x => !users.ContainsKey(x)))
                mentions += $"<@{id}> ";

            await channel.SendMessageAsync(mentions, embed: builder.Build());
        }
    }
}
