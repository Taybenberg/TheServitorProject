﻿using BumperService;
using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnBumperNotifyAsync(BumpNotificationContainer container)
        {
            IMessageChannel channel = _client.GetChannel(_bumpChannelId) as IMessageChannel;

            var builder = new EmbedBuilder()
                .WithColor(0xFF6E00)
                .WithDescription("Саме час **!bump**-нути :fire:");

            if (container.UserCooldowns.Count > 0)
                builder.Description += "\nКулдаун до:\n" + string.Join('\n',
                    container.UserCooldowns.OrderBy(x => x.Value)
                    .Select(user => $"<@{user.Key}> – *{user.Value.ToString("HH:mm")}*"));

            var component = new ComponentBuilder()
                .WithButton("Підписатися на сповіщення", "BumpNotificationsSubscribe", ButtonStyle.Success)
                .WithButton("Відписатися від сповіщень", "BumpNotificationsUnsubscribe", ButtonStyle.Danger);

            await channel.SendMessageAsync(string.Join(' ', container.PingableUserIDs.Select(y => $"<@{y}>")),
                embed: builder.Build(), components: component.Build());
        }
    }
}