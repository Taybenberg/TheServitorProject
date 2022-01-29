using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnSelectMenuExecutedAsync(SocketMessageComponent component)
        {
            if (_activityChannelIDs.Any(x => x == component.Channel.Id))
                await ActivitySelectMenuExecutedAsync(component);
        }
    }
}
