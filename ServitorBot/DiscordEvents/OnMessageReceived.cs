using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
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
            else if (await ServiceMessagesAsync(message))
                return;
            else if (_lulzChannelIDs.Any(x => x == message.Channel.Id))
                await LulzChannelManagerAsync(message);
            else if (_musicChannelIDs.Any(x => x == message.Channel.Id))
                await MusicPlayerMessageReceivedAsync(message);
            else if (_activityChannelIDs.Any(x => x == message.Channel.Id))
                await ActivityMessageReceivedAsync(message);
        }
    }
}
