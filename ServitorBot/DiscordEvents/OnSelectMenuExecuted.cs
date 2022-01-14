using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnSelectMenuExecutedAsync(SocketMessageComponent component)
        {
            if (_activityChannelId.Any(x => x == component.Channel.Id))
                await ActivitySelectMenuExecutedAsync(component);
        }
    }
}
