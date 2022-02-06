using Discord;
using ServitorBot.BotCommands.TextCommands;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task OnMessageReceivedAsync(IMessage message)
        {
            if (_bumpChannelIDs.Any(x => x == message.Channel.Id) && message.Author.IsBot && message.Embeds.Count > 0)
            {
                await InitBumpAsync(message);
                return;
            }

            if (message.Author.IsBot)
                return;
            else if (await new ServiceCommandsManager(_client).ProcessServiceCommandAsync(message))
                return;
            else if (_lulzChannelIDs.Any(x => x == message.Channel.Id))
                await new ServiceCommandsManager(_client).LulzChannelProcessMessageAsync(message);
            else if (_musicChannelIDs.Any(x => x == message.Channel.Id))
                await MusicPlayerMessageReceivedAsync(message);
            else if (_activityChannelIDs.Any(x => x == message.Channel.Id))
                await ActivityMessageReceivedAsync(message);
        }
    }
}
