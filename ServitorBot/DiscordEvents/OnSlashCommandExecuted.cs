using Discord.WebSocket;
using ServitorBot.BotCommands;
using ServitorBot.BotCommands.SlashCommands;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task OnSlashCommandExecutedAsync(SocketSlashCommand command)
        {
            if (_mainChannelIDs.Any(x => x == command.Channel.Id))
            {
                var slashCommand = CommandHelper.SlashCommands.FirstOrDefault(x => x.CommandName == command.CommandName);

                if (slashCommand is not null)
                    await slashCommand.ExecuteCommandAsync(command, _scopeFactory);
            }
            else if (_activityChannelIDs.Any(x => x == command.Channel.Id) && command.CommandName == new OrganizeActivity().CommandName)
                await ActivitySlashCommandExecutedAsync(command);
            else
                await command.RespondAsync(embed: CommandHelper.WrongChannelBuilder.Build(), ephemeral: true);
        }
    }
}
