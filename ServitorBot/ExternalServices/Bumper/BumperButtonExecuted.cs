﻿using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task BumperButtonExecutedAsync(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "BumpNotificationsSubscribe":
                    {
                        await _bumper.SubscribeUserAsync(component.User.Id);

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0x48BA59))
                            .WithDescription($"{component.User.Mention} тепер отримуватиме сповіщення про **bump**");

                        await component.RespondAsync(embed: builder.Build(), ephemeral: true);
                    }
                    break;

                case "BumpNotificationsUnsubscribe":
                    {
                        await _bumper.UnSubscribeUserAsync(component.User.Id);

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xEE1B24))
                            .WithDescription($"{component.User.Mention} більше не отримуватиме сповіщення про **bump**");

                        await component.RespondAsync(embed: builder.Build(), ephemeral: true);
                    }
                    break;

                default: break;
            }
        }
    }
}
