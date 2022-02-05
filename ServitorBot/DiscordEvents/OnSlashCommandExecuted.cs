using Discord.WebSocket;
using ServitorDiscordBot.BotCommands;
using ServitorDiscordBot.BotCommands.SlashCommands;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
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
