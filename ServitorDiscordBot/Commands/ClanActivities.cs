using DataProcessor.Localization;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetClanActivitiesAsync(IMessage message)
        {
            var database = getDatabase();

            var builder = GetBuilder(MessagesEnum.ClanActivities, message);

            var acts = await database.GetActivitiesAsync();

            builder.Description = $"Нічого собі! **{acts.Count()}** активностей на рахунку клану!\n\n***По типу активності:***";

            List<(BungieNetApi.Enums.ActivityType ActivityType, int Count)> counter = new();

            foreach (var type in acts.Select(x => x.ActivityType).Distinct())
                counter.Add((type, acts.Count(x => x.ActivityType == type)));

            foreach (var count in counter.OrderByDescending(x => x.Count))
            {
                var mode = TranslationDictionaries.ActivityNames[count.ActivityType];

                builder.Description += $"\n**{mode[0]}** | {mode[1]} – ***{count.Count}***";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
