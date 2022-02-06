using Discord.WebSocket;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task OnButtonExecutedAsync(SocketMessageComponent component)
        {
            if (_bumpChannelIDs.Any(x => x == component.Channel.Id))
                await BumperButtonExecutedAsync(component);
            else if (_activityChannelIDs.Any(x => x == component.Channel.Id))
                await ActivityButtonExecutedAsync(component);
            else if (_musicChannelIDs.Any(x => x == component.Channel.Id))
                await MusicPlayerButtonExecutedAsync(component);
        }
    }
}
