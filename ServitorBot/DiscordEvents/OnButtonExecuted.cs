using Discord.WebSocket;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnButtonExecutedAsync(SocketMessageComponent component)
        {
            if (component.Channel.Id == _bumpChannelId)
                await BumperButtonExecutedAsync(component);
        }
    }
}
