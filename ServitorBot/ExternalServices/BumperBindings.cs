using BumperService;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task BumperButtonExecuted(SocketMessageComponent component)
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

        private async Task InitBumpAsync(IMessage message)
        {
            var embed = message.Embeds.FirstOrDefault();

            if (embed is not null)
            {
                if (embed.Description?.Contains("Server bumped by") ?? false)
                {
                    var mention = Regex.Match(embed.Description, "(?<=\\<@)\\D?(\\d+)(?=\\>)").Groups[1].Value;

                    var nextBump = await _bumper.RegisterBumpAsync(ulong.Parse(mention));

                    var builder = new EmbedBuilder()
                        .WithColor(0xFF6E00)
                        .WithDescription($"{DataProcessor.DiscordEmoji.EmojiContainer.BumpTimer} {nextBump.ToString("HH:mm")}");

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
            }
        }

        private async Task BumperNotify(BumpNotificationContainer container)
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
                .WithButton("Підписатися на сповіщення", "BumpNotificationsSubscribe", ButtonStyle.Secondary,
                Emote.Parse(DataProcessor.DiscordEmoji.EmojiContainer.Check))
                .WithButton("Відписатися від сповіщень", "BumpNotificationsUnsubscribe", ButtonStyle.Secondary,
                Emote.Parse(DataProcessor.DiscordEmoji.EmojiContainer.UnCheck));

            await channel.SendMessageAsync(string.Join(' ', container.PingableUserIDs.Select(y => $"<@{y}>")), 
                embed: builder.Build(), components: component.Build());
        }
    }
}
