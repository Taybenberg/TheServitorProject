using Discord.WebSocket;
using ServitorDiscordBot.BotCommands;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnSlashCommandExecutedAsync(SocketSlashCommand command)
        {
            if (!_mainChannelIDs.Any(x => x == command.Channel.Id))
                return;

            var slashCommand = ISlashCommand.SlashCommands.FirstOrDefault(x => x.CommandName == command.CommandName);

            if (slashCommand is not null)
                await slashCommand.ExecuteCommandAsync(command, _scopeFactory);
        }
    }
}
