using Discord.WebSocket;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task OnSelectMenuExecutedAsync(SocketMessageComponent component)
        {
            if (_activityChannelIDs.Any(x => x == component.Channel.Id))
                await ActivitySelectMenuExecutedAsync(component);
        }
    }
}
