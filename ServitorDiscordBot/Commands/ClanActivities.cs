using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetClanActivitiesAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.ClanActivities, message);

            var counter = await getStatsFactory().GetClanActivitiesAsync();

            builder.Description = $"Нічого собі! **{counter.Count}** активностей на рахунку клану!\n\n***По типу активності:***";

            foreach (var count in counter.Modes)
                builder.Description += $"\n**{count.Modes[0]}** | {count.Modes[1]} – ***{count.Count}***";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
