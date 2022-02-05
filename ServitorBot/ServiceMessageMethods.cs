using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private int GetWeekNumber() =>
            1;

        private (DateTime?, string) GetPeriod(string period) =>
            period switch
            {
                "тиждень" => (DateTime.UtcNow.AddDays(-7), " за останній тиждень"),
                "місяць" => (DateTime.UtcNow.AddMonths(-1), " за останній місяць"),
                _ => (null, " за весь час")
            };

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

        private EmbedBuilder GetBuilder(MessagesEnum messagesEnum, IMessage message, bool getFooter = true, string userName = null)
        {
            var builder = new EmbedBuilder();

            //builder.Title = GetTitle(messagesEnum, message, userName);

            builder.Color = GetColor(messagesEnum);

            if (getFooter)
            {
                var footer = new EmbedFooterBuilder();

                footer.IconUrl = _client.CurrentUser.GetAvatarUrl();
                footer.Text = $"Ваш відданий {_client.CurrentUser.Username} | !donate – підтримати автора";

                builder.Footer = footer;
            }

            return builder;
        }

        private async Task SendDonateMessageAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = Color.Gold;

            builder.ThumbnailUrl = _client.GetUser(228896926991515649).GetAvatarUrl();

            builder.Title = "Підтримати автора";

            builder.Description = $"Ви завжди можете підтримати <@228896926991515649> чашкою кави на сервісі [Buy Me a Coffee](https://www.buymeacoffee.com/servitor).";

            await channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
