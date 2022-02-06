using ServitorBot.BotCommands;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task RegisterSlashCommandsAsync()
        {
            await _client.GetGuild(799280582512476210)
                .BulkOverwriteApplicationCommandAsync(CommandHelper.SlashCommands
                .Select(x => x.SlashCommand.Build()).ToArray());
        }
    }
}
