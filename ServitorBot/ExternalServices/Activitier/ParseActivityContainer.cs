using ActivityService;
using CommonData.Activities;
using Discord;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private EmbedBuilder ParseActivityContainer(ActivityContainer container)
        {
            (var icon, var activityName) = Activity
                .GetActivityInfo(container.ActivityType, container.ActivityName);

            var ftSize = Activity.GetFireteamSize(container.ActivityType);

            var users = new List<EmbedFieldBuilder>();
            if (container.Users.Count() > 0)
            {
                var leader = container.Users.FirstOrDefault();
                users.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Організатор збору",
                    Value = $"<@{leader}>"
                });

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
            }

            return new EmbedBuilder()
                .WithColor(new Color(0xFFFFFF))
                .WithThumbnailUrl(Emote.Parse(icon).Url)
                .WithTitle($"{activityName} @ {container.PlannedDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm")}")
                .WithDescription(container.Description)
                .WithFields(users);
        }
    }
}