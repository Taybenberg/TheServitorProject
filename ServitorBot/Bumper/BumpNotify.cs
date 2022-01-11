using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BumperService;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Bumper_Notify(BumpNotificationContainer container)
        {
            _logger.LogInformation($"{DateTime.Now} Bump notification");

            IMessageChannel channel = _client.GetChannel(_bumpChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.BumpNotification, null, false);

            builder.Description = "Саме час **!bump**-нути :fire:";

            if (container.UserCooldowns.Count > 0)
            {
                builder.Description += "\nКулдаун до:";

                foreach (var user in container.UserCooldowns.OrderBy(x => x.Value))
                    builder.Description += $"\n<@{user.Key}> – *{user.Value.ToString("HH:mm")}*";
            }

            string mentions = string.Join(" ", container.PingableUserIDs.Select(y => $"<@{y}>"));

            await channel.SendMessageAsync(mentions, embed: builder.Build());
        }
    }
}
