using Discord;
using Discord.WebSocket;

namespace ServitorBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private bool CheckDeleteMessagesPermission(IUser user)
        {
            var socketUser = user as SocketGuildUser;

            return socketUser.GuildPermissions.ManageMessages;
        }

        public static async Task SendTemporaryMessageAsync(IMessage message, EmbedBuilder builder)
        {
            var msg = await message.Channel.SendMessageAsync(embed: builder.Build());

            await Task.Delay(5000);

            await DeleteMessageAsync(msg);
        }

        public static async Task DeleteMessageAsync(IMessage message)
        {
            try
            {
                await message.DeleteAsync();
            }
            catch { }
        }
    }
}
