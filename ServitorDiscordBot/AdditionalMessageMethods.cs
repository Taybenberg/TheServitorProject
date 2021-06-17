using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessage, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            if (deleteSenderMessage)
                await message.DeleteAsync();

            await method(message);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessage, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            if (deleteSenderMessage)
                await message.DeleteAsync();

            await method(message, arg);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessageChannel, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            if (deleteSenderMessage)
                await message.DeleteAsync();

            await method(message.Channel);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessageChannel, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            if (deleteSenderMessage)
                await message.DeleteAsync();

            await method(message.Channel, arg);

            await wait.DeleteAsync();
        }

        private async Task<IUserMessage> GetWaitMessageAsync(IMessage message)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Wait);

            builder.Description = $"Виконую ваш запит \"{message.Content}\", на це знадобиться трохи часу…";

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
