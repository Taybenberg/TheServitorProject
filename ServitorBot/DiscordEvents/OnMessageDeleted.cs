using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnMessageDeletedAsync(Cacheable<IMessage, ulong> message, IMessageChannel channel)
        {
            /*
            if (channel.Id != _raidChannelId)
                return;

            await _raidManager.TryRemoveRaid(message.Id);
            */
        }
    }
}
