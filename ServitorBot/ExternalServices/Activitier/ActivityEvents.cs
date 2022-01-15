using ActivityService;
using CommonData.Activities;
using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnActivityUpdatedAsync(ActivityContainer activity)
        {
            IMessageChannel channel = _client.GetChannel(activity.ChannelID) as IMessageChannel;

            var builder = ParseActivityContainer(activity);

            var componentBuilder = new ComponentBuilder()
                .WithButton("Підписатися", "ActivitierSubscribe", ButtonStyle.Success)
                .WithButton("Відписатися", "ActivitierUnsubscribe", ButtonStyle.Danger);

            await channel.ModifyMessageAsync(activity.ActivityID, msg =>
            {
                msg.Content = $"<@&{_destinyRoleId}>";
                msg.Embed = builder.Build();
                msg.Components = componentBuilder.Build();
            });
        }

        private async Task OnActivityNotificationAsync(ActivityContainer activity)
        {
            var builder = ParseActivityContainer(activity);

            var ftSize = Activity.GetFireteamSize(activity.ActivityType);

            string notes = "!";
            if (ftSize < activity.Users.Count())
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
            IMessageChannel channel = await _client.GetChannelAsync(activity.ChannelID) as IMessageChannel;

            try
            {
                await channel.DeleteMessageAsync(activity.ActivityID);
            }
            catch { }

            if (activity.PlannedDate < DateTime.UtcNow)
            {
                var builder = ParseActivityContainer(activity);

                foreach (var userID in activity.Users.Take(Activity.GetFireteamSize(activity.ActivityType)))
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
    }
}
