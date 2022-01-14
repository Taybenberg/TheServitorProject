using BumperService;
using CommonData.DiscordEmoji;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivityButtonExecutedAsync(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "ActivitierSubscribe":
                    {
                        await component.DeferAsync();
                    }
                    break;

                case "ActivitierUnsubscribe":
                    {
                        await component.DeferAsync();
                    }
                    break;

                default: break;
            }
        }
    }
}
