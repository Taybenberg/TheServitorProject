using Discord;
using Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetUserActivitiesAsync(IMessage message)
        {
            var database = getDatabase();

            var user = await database.GetUserActivitiesAsync(message.Author.Id);

            if (user is null)
            {
                await UserIsNotRegisteredAsync(message.Channel);

                return;
            }

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.MyActivities);

            builder.Title = $"Активності {message.Author.Username}";

            var acts = user.Characters.SelectMany(c => c.ActivityUserStats);

            builder.Description = $"Неймовірно! **{acts.Count()}** активностей на рахунку {message.Author.Mention}! Так тримати!\n\n***По класах:***";

            foreach (var c in user.Characters.OrderByDescending(x => x.ActivityUserStats.Count))
                builder.Description += $"\n**{Localization.ClassNames[c.Class]}** – ***{c.ActivityUserStats.Count}***";

            builder.Description += "\n\n***По типу активності:***";

            List<(BungieNetApi.ActivityType ActivityType, int Count)> counter = new();

            foreach (var type in acts.Select(x => x.Activity.ActivityType).Distinct())
                counter.Add((type, acts.Count(x => x.Activity.ActivityType == type)));

            foreach (var count in counter.OrderByDescending(x => x.Count))
            {
                var mode = Localization.ActivityNames[count.ActivityType];

                builder.Description += $"\n**{mode[0]}** | {mode[1]} – ***{count.Count}***";
            }

            builder.Footer = GetFooter();

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task GetUserPartnersAsync(IMessage message)
        {
            var database = getDatabase();

            if (!database.IsDiscordUserRegistered(message.Author.Id))
            {
                await UserIsNotRegisteredAsync(message.Channel);

                return;
            }

            var partners = await database.GetUserPartnersAsync(message.Author.Id);

            var builder = new EmbedBuilder();

            builder.Title = $"Побратими {message.Author.Username}";

            builder.Footer = GetFooter();

            if (!partners.Any())
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Я не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали у цьому році.";
            }
            else
            {
                builder.Color = GetColor(MessagesEnum.MyPartners);

                builder.Description = string.Empty;

                foreach (var p in partners)
                    builder.Description += $"**{p.UserName}** – ***{p.Count}***\n";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task GetClanActivitiesAsync(IMessageChannel channel)
        {
            var database = getDatabase();

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.ClanActivities);

            builder.Title = $"Активності клану {(channel as IGuildChannel).Guild.Name}";

            var acts = await database.Activities.ToListAsync();

            builder.Description = $"Нічого собі! **{acts.Count}** активностей на рахунку клану!\n\n***По типу активності:***";

            List<(BungieNetApi.ActivityType ActivityType, int Count)> counter = new();

            foreach (var type in acts.Select(x => x.ActivityType).Distinct())
                counter.Add((type, acts.Count(x => x.ActivityType == type)));

            foreach (var count in counter.OrderByDescending(x => x.Count))
            {
                var mode = Localization.ActivityNames[count.ActivityType];

                builder.Description += $"\n**{mode[0]}** | {mode[1]} – ***{count.Count}***";
            }

            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task FindSuspiciousAsync(IMessageChannel channel, bool nigthfalls)
        {
            var database = getDatabase();

            var apiClient = getApiClient();

            ConcurrentDictionary<DateTime, string> activityDetails = new();

            var activities = nigthfalls ? await database.GetSuspiciousNightfallsOnlyAsync(DateTime.Now.AddDays(-7)) : await database.GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime.Now.AddDays(-7));

            Parallel.ForEach(activities, (activity) =>
            {
                string details = $" {activity.ActivityType}";

                var act = activity.GetActivityAdditionalDetailsAsync(apiClient).Result;

                if (activity.ActivityType == BungieNetApi.ActivityType.ScoredNightfall)
                {
                    var u = act.ActivityUserStats.FirstOrDefault();

                    if (u is not null)
                        details += $" {u.TeamScore}";
                }

                List<string> members = new();

                foreach (var u in act.ActivityUserStats)
                {
                    var memberClans = apiClient.GetUserClansAsync(u.MembershipType, u.MembershipId).Result;

                    members.Add($"\n{u.DisplayName} {HttpUtility.HtmlDecode(string.Join(",", memberClans))}");
                }

                details += string.Join(string.Empty, members.Distinct());

                activityDetails.TryAdd(activity.Period, details);
            });

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Suspicious);

            builder.Title = $"Виявлено активностей: {activityDetails.Count}";

            string list = "Увага, чутливим не читати! Останні активності:\n||";

            foreach (var act in activityDetails.OrderByDescending(x => x.Key))
            {
                if ((list + act + "\n\n||").Length < 2000)
                    list += act + "\n\n";
            }

            list += "||";

            builder.Description = list;

            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
