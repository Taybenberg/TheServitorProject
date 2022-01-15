using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivityMessageDeletedAsync(ulong messageID)
        {
            await _activityManager.DisableActivityAsync(messageID);
        }
    }
}