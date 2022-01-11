using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Event_Delete(ulong messageID)
        {
            IMessageChannel channel = await _client.Rest.GetChannelAsync(_raidChannelId) as IMessageChannel;

            try
            {
                await channel.DeleteMessageAsync(messageID);
            }
            catch { }
        }
    }
}
