using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private bool CheckModerationRole(IUser user)
        {
            var sUser = user as SocketGuildUser;

            return sUser.Roles.Any(x =>
            x.Name.ToLower() is "administrator" ||
            x.Name.ToLower() is "moderator" ||
            x.Name.ToLower() is "raid lead") ||
            sUser.Guild.OwnerId == user.Id;
        }

        private async Task LulzChannelManagerAsync(IMessage message)
        {
            await Task.Delay(1000);

            try
            {
                var channel = await _client.Rest.GetChannelAsync(message.Channel.Id) as Discord.Rest.IRestMessageChannel;

                var msg = await channel.GetMessageAsync(message.Id);

                if (msg.Source != MessageSource.User || msg.Attachments.Count > 0 || msg.Embeds.Count > 0)
                    return;

                await msg.DeleteAsync();
            }
            catch { }
        }

        private async Task<(IGuild, IMessageChannel, IMessage)> GetChannelMessageAsync(string link)
        {
            var strs = link.Split('/');

            if (strs.Length < 4)
                return (null, null, null);

            var glid = ulong.Parse(strs[^3]);
            var chid = ulong.Parse(strs[^2]);
            var msid = ulong.Parse(strs[^1]);

            var gl = _client.GetGuild(glid) as IGuild;
            var ch = await gl.GetChannelAsync(chid) as IMessageChannel;
            var ms = await ch.GetMessageAsync(msid);

            return (gl, ch, ms);
        }

        private async Task SendTemporaryMessageAsync(IMessage message, string text)
        {
            var msg = await message.Channel.SendMessageAsync(text);

            await DeleteMessageAsync(message);

            await Task.Delay(5000);

            await DeleteMessageAsync(msg);
        }

        private async Task SendTemporaryMessageAsync(IMessage message, EmbedBuilder builder)
        {
            var msg = await message.Channel.SendMessageAsync(embed: builder.Build());

            await DeleteMessageAsync(message);

            await Task.Delay(5000);

            await DeleteMessageAsync(msg);
        }

        private async Task DeleteMessageAsync(IMessage message, bool confirmation = true)
        {
            if (confirmation)
            {
                try
                {
                    await message.DeleteAsync();
                }
                catch { }
            }
        }
    }
}
