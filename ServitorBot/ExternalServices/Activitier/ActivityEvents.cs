using ActivityService;
using CommonData.Activities;
using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnActivityUpdatedAsync(ActivityContainer activity)
        {
            IMessageChannel channel = _client.GetChannel(activity.ChannelID) as IMessageChannel;

            var builder = ParseActivityContainer(activity);

            var componentBuilder = new ComponentBuilder()
                .WithButton(customId: "ActivitierSubscribe", style: ButtonStyle.Primary, emote: new Emoji("\U00002795"));

            await channel.ModifyMessageAsync(activity.ActivityID, msg =>
            {
                msg.Content = $"<@&{_destinyRoleID}>";
                msg.Embed = builder.Build();
                msg.Components = componentBuilder.Build();
            });
        }

        private async Task OnActivityNotificationAsync(ActivityContainer activity)
        {
            var builder = ParseActivityContainer(activity);

            var ftSize = Activity.GetFireteamSize(activity.ActivityType);

            string notes = "!";
            if (ftSize > activity.Users.Count())
                notes = ", але необхідна кількість ґардіанів не назбиралася!";

            foreach (var userID in activity.Users.Take(ftSize))
            {
                try
                {
                    var user = await _client.Rest.GetUserAsync(userID);

                    await user.SendMessageAsync($"Ґардіане, незабаром у вас розпочнеться активність{notes}", embed: builder.Build());
                }
                catch { }
            }
        }

        private async Task OnActivityDisabledAsync(ActivityContainer activity)
        {
            try
            {
                IMessageChannel channel = await _client.GetChannelAsync(activity.ChannelID) as IMessageChannel;

                await channel.DeleteMessageAsync(activity.ActivityID);
            }
            catch { }

            if (activity.PlannedDate > DateTime.UtcNow)
            {
                var builder = ParseActivityContainer(activity);

                foreach (var userID in activity.Users.Take(Activity.GetFireteamSize(activity.ActivityType)).Skip(1))
                {
                    try
                    {
                        var user = await _client.Rest.GetUserAsync(userID);

                        await user.SendMessageAsync($"Ґардіане, вашу активність було скасовано.", embed: builder.Build());
                    }
                    catch { }
                }
            }
        }

        private async Task OnActivityCreatedAsync(ActivityContainer activity)
        {
            var builder = ParseActivityContainer(activity);

            foreach (var userID in activity.Users.Take(Activity.GetFireteamSize(activity.ActivityType)).Skip(1))
            {
                try
                {
                    var user = await _client.Rest.GetUserAsync(userID);

                    await user.SendMessageAsync($"Ґардіане, вас записали у активність.", embed: builder.Build());
                }
                catch { }
            }
        }

        private async Task OnActivityRescheduledAsync(ActivityContainer activity)
        {
            var builder = ParseActivityContainer(activity);

            foreach (var userID in activity.Users.Take(Activity.GetFireteamSize(activity.ActivityType)).Skip(1))
            {
                try
                {
                    var user = await _client.Rest.GetUserAsync(userID);

                    await user.SendMessageAsync($"Ґардіане, вашу активність було перенесено.", embed: builder.Build());
                }
                catch { }
            }
        }
    }
}
