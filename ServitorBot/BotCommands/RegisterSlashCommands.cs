using ServitorDiscordBot.BotCommands;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task RegisterSlashCommandsAsync()
        {
            await _client.GetGuild(799280582512476210)
                .BulkOverwriteApplicationCommandAsync(ISlashCommand.SlashCommands
                .Select(x => x.SlashCommand.Build()).ToArray());
        }
    }
}
