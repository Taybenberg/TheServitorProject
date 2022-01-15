using ActivityService;
using CommonData.Activities;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitActivityAsync(ActivityContainer container)
        {
            container.PlannedDate = container.PlannedDate.ToUniversalTime();

            (var icon, var activityName) = Activity
                .GetActivityInfo(container.ActivityType, container.ActivityName);

            var ftSize = Activity.GetFireteamSize(container.ActivityType);
            var users = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Організатор збору",
                    Value = $"<@{container.Users.FirstOrDefault()}>"
                }
            };

            var fireteam = container.Users.Skip(1).Take(ftSize - 1);
            if (fireteam.Count() > 0)
                users.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Бойова група",
                    Value = string.Join("\n", fireteam.Select(x => $"<@{x}>"))
                });

            var reserve = container.Users.Skip(ftSize);
            if (reserve.Count() > 0)
                users.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Лава запасних",
                    Value = string.Join("\n", reserve.Select(x => $"<@{x}>"))
                });

            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFFFFFF))
                .WithThumbnailUrl(Emote.Parse(icon).Url)
                .WithTitle($"{activityName} @ {container.PlannedDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm")}")
                .WithDescription(container.Description)
                .WithFields(users);

            IMessageChannel channel = _client.GetChannel(container.ChannelID) as IMessageChannel;

            var componentBuilder = new ComponentBuilder()
                .WithButton("Підписатися", "ActivitierSubscribe", ButtonStyle.Secondary, Emote.Parse(CommonData.DiscordEmoji.Emoji.Check))
                .WithButton("Відписатися", "ActivitierUnsubscribe", ButtonStyle.Secondary, Emote.Parse(CommonData.DiscordEmoji.Emoji.UnCheck));

            await channel.SendMessageAsync($"<@&{_destinyRoleId}>", embed: builder.Build(), components: componentBuilder.Build());
        }
    }
}