using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Event_Update(RaidContainer container)
        {
            IMessageChannel channel = _client.GetChannel(_raidChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            container.DecorateBuilder(builder);

            try
            {
                await channel.ModifyMessageAsync(container.ID, msg => msg.Embed = builder.Build());
            }
            catch { }
        }
    }
}
