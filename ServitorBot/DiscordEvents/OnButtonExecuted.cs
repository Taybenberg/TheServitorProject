using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnButtonExecutedAsync(SocketMessageComponent component)
        {
            if (component.Channel.Id == _bumpChannelId)
                await BumperButtonExecutedAsync(component);
            else if (_activityChannelId.Any(x => x == component.Channel.Id))
                await ActivityButtonExecutedAsync(component);
            else if (component.Channel.Id == _musicChannelId)
                await MusicPlayerButtonExecutedAsync(component);
        }
    }
}
