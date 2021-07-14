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
            var activities = await getStatsFactory().GetMyActivitiesAsync(message.Author.Id);

            if (!activities.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyActivities, message);

            builder.Description = $"Неймовірно! **{activities.Count}** активностей на рахунку {message.Author.Mention}! Так тримати!\n" +
                $"\n***По класах:***\n{string.Join("\n", activities.Classes.Select(x => $"**{x.Class}** – ***{x.Count}***"))}\n" +
                $"\n***По типу активності:***\n{string.Join("\n", activities.Modes.Select(x => $"**{x.Modes[0]}** | {x.Modes[1]} – ***{x.Count}***"))}";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
