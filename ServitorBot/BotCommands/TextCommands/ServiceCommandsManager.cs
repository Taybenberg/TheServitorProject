using Discord.WebSocket;

namespace ServitorBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private readonly DiscordSocketClient _client;

        public ServiceCommandsManager(DiscordSocketClient client) => _client = client;
    }
}
