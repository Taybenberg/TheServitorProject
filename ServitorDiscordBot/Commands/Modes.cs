using DataProcessor.Localization;
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

            builder.Description = string.Empty;

            foreach (var mode in TranslationDictionaries.StatsActivityNames.Values.OrderBy(x => x[0]))
                builder.Description += $"**{mode[0]}** | {mode[1]}\n";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
