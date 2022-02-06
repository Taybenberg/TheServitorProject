using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private readonly DiscordSocketClient _client;

        public ServiceCommandsManager(DiscordSocketClient client) => _client = client;

        public async Task<bool> ProcessServiceCommandAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case "!secret_help":
                    await message.Channel.SendMessageAsync(embeds: HelpEmbeds);
                    return true;

                case "!donate":
                    await message.Channel.SendMessageAsync(embed: DonateEmbedBuilder.Build());
                    return true;
            }

            return false;
        }
    }
}
