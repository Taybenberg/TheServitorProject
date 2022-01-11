using BumperService;
using Discord;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitBumpAsync(IMessage message)
        {
            var embed = message.Embeds.FirstOrDefault();

            if (embed is not null)
            {
                if (embed.Description?.Contains("Server bumped by") ?? false)
                {
                    var mention = Regex.Match(embed.Description, "(?<=\\<@)\\D?(\\d+)(?=\\>)").Groups[1].Value;

                    var nextBump = await _bumper.RegisterBumpAsync(ulong.Parse(mention));

                    var builder = GetBuilder(MessagesEnum.Bumped, message, false);

                    builder.Description = $"<:bump_timer:867070921452552213> {nextBump.ToString("HH:mm")}";

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
            }
        }

        private async Task Bumper_Notify(BumpNotificationContainer container)
        {
            IMessageChannel channel = _client.GetChannel(_bumpChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.BumpNotification, null, false);

            builder.Description = "Саме час **!bump**-нути :fire:";

            if (container.UserCooldowns.Count > 0)
                builder.Description += "\nКулдаун до:\n" + string.Join('\n',
                    container.UserCooldowns.OrderBy(x => x.Value)
                    .Select(user => $"<@{user.Key}> – *{user.Value.ToString("HH: mm")}*"));

            await channel.SendMessageAsync(string.Join(' ', container.PingableUserIDs.Select(y => $"<@{y}>")), embed: builder.Build());
        }
    }
}
