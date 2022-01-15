using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnMessageDeletedAsync(ulong messageID, ulong channelID)
        {
            if (_activityChannelId.Any(x => x == channelID))
               await ActivityMessageDeletedAsync(messageID);
        }
    }
}
