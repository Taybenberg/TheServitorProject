using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ExecuteWaitMessageAsync(SocketMessage message, Func<SocketMessage, Task> method)
        {
            var wait = await GetWaitMessageAsync(message.Channel);

            await method(message);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(SocketMessage message, Func<SocketMessage, T, Task> method, T arg)
        {
            var wait = await GetWaitMessageAsync(message.Channel);

            await method(message, arg);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync(IMessageChannel channel, Func<IMessageChannel, Task> method)
        {
            var wait = await GetWaitMessageAsync(channel);

            await method(channel);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessageChannel channel, Func<IMessageChannel, T, Task> method, T arg)
        {
            var wait = await GetWaitMessageAsync(channel);

            await method(channel, arg);

            await wait.DeleteAsync();
        }

        private async Task<IUserMessage> GetWaitMessageAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Wait);

            builder.Description = "Виконую ваш запит, на це знадобиться трохи часу…";

            return await channel.SendMessageAsync(embed: builder.Build());
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
