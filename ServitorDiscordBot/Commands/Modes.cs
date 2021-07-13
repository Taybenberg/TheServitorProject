using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetModesAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.Modes, message);

            var modes = await getStatsFactory().GetModesAsync();

            builder.Description = string.Join("\n", modes.Select(x => $"**{x[0]}** | {x[1]}"));

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
