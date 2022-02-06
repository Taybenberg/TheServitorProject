using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private bool CheckDeleteMessagesPermission(IUser user)
        {
            var socketUser = user as SocketGuildUser;

            return socketUser.GuildPermissions.ManageMessages;
        }
    }
}
