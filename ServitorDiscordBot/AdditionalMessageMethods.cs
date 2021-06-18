using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private bool CheckModerationRole(IUser user) => 
            ((SocketGuildUser)user).Roles.Any(x => 
            x.Name.ToLower().StartsWith("admin") || 
            x.Name.ToLower().StartsWith("moder") ||
            x.Name.ToLower() is "raid lead" ||
            x.Name.ToLower() is "old");

        private async Task<(IMessageChannel, IMessage)> GetChannelMessageAsync(string link)
        {
            var strs = link.Split('/');

            if (strs.Length < 4)
                return (null, null);

            var chid = ulong.Parse(strs[^2]);
            var msid = ulong.Parse(strs[^1]);

            var ch = _client.GetChannel(chid) as IMessageChannel;
            var ms = await ch.GetMessageAsync(msid);

            return (ch, ms);
        }

        private async Task SendTemporaryMessageAsync(IMessage message, string text)
        {
            var msg = await message.Channel.SendMessageAsync(text);

            try
            {
                await message.DeleteAsync();
            }
            catch (Exception) { }

            await Task.Delay(5000);

            await msg.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessage, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessage, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message, arg);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessageChannel, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message.Channel);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessageChannel, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message.Channel, arg);

            await wait.DeleteAsync();
        }

        private async Task DeleteMessageAsync(IMessage message, bool del)
        {
            if (del)
            {
                try
                {
                    await message.DeleteAsync();
                }
                catch (Exception) { }
            }
        }

        private async Task<IUserMessage> GetWaitMessageAsync(IMessage message)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Wait);

            builder.Description = $"Виконую ваш запит \"{message.Content.ToLower()}\", на це знадобиться трохи часу…";

            return await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private EmbedFooterBuilder GetFooter()
        {
            var footer = new EmbedFooterBuilder();

            footer.IconUrl = _client.CurrentUser.GetAvatarUrl();
            footer.Text = $"Ваш відданий {_client.CurrentUser.Username}";

            return footer;
        }
    }
}
