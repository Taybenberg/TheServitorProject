using DataProcessor;
using Discord;
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
        private async Task GetSuspiciousActivitiesAsync(IMessage message, bool nigthfalls)
        {
            var database = getDatabase();

            var apiClient = getApiClient();

            ConcurrentDictionary<DateTime, string> activityDetails = new();

            var activities = nigthfalls ? await database.GetSuspiciousNightfallsOnlyAsync(DateTime.Now.AddDays(-7)) : await database.GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime.Now.AddDays(-7));

            Parallel.ForEach(activities, (activity) =>
            {
                string details = $" {activity.ActivityType}";

                var act = activity.GetDestinyActivity(apiClient);

                if (activity.ActivityType == BungieNetApi.Enums.ActivityType.ScoredNightfall)
                {
                    var u = act.UserStats.FirstOrDefault();

                    if (u is not null)
                        details += $" {u.TeamScore}";
                }

                List<string> members = new();

                foreach (var u in act.UserStats)
                {
                    var memberClans = apiClient.EntityFactory.GetUser(u.MembershipID, u.MembershipType).GetUserClansAsync().Result;

                    members.Add($"\n{u.DisplayName} {HttpUtility.HtmlDecode(string.Join(",", memberClans.Select(x => $"({x.ClanSign}, {x.ClanName})")))}");
                }

                details += string.Join(string.Empty, members.Distinct());

                activityDetails.TryAdd(activity.Period, details);
            });

            var builder = GetBuilder(MessagesEnum.Suspicious, message);

            string list = $"Виявлено активностей за останні 7 днів: {activityDetails.Count}\nУвага, чутливим не читати! Останні активності:\n||";

            foreach (var act in activityDetails.OrderByDescending(x => x.Key))
            {
                if ((list + act + "\n\n||").Length < 2000)
                    list += act + "\n\n";
            }

            list += "||";

            builder.Description = list;

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
