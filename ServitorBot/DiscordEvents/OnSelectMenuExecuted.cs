using Discord.WebSocket;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnSelectMenuExecutedAsync(SocketMessageComponent component)
        {
            if (component.Channel.Id == _raidChannelId)
                await RaidSelectMenuExecutedAsync(component);
        }
    }
}
