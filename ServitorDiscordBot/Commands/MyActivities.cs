using DataProcessor.Localization;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetMyActivitiesAsync(IMessage message)
        {
            var database = getDatabase();

            var user = await database.GetUserActivitiesAsync(message.Author.Id);

            if (user is null)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyActivities, message);

            var acts = user.Characters.SelectMany(c => c.ActivityUserStats);

            builder.Description = $"Неймовірно! **{acts.Count()}** активностей на рахунку {message.Author.Mention}! Так тримати!\n\n***По класах:***";

            foreach (var c in user.Characters.OrderByDescending(x => x.ActivityUserStats.Count))
                builder.Description += $"\n**{TranslationDictionaries.ClassNames[c.Class]}** – ***{c.ActivityUserStats.Count}***";

            builder.Description += "\n\n***По типу активності:***";

            List<(BungieNetApi.Enums.ActivityType ActivityType, int Count)> counter = new();

            foreach (var type in acts.Select(x => x.Activity.ActivityType).Distinct())
                counter.Add((type, acts.Count(x => x.Activity.ActivityType == type)));

            foreach (var count in counter.OrderByDescending(x => x.Count))
            {
                var mode = TranslationDictionaries.ActivityNames[count.ActivityType];

                builder.Description += $"\n**{mode[0]}** | {mode[1]} – ***{count.Count}***";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
