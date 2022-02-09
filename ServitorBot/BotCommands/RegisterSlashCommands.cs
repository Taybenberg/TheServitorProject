using ServitorBot.BotCommands;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task RegisterSlashCommandsAsync()
        {
            await _client
                .BulkOverwriteGlobalApplicationCommandsAsync(CommandHelper
                .SlashCommands.Select(x => x.SlashCommand.Build()).ToArray());
        }
    }
}
