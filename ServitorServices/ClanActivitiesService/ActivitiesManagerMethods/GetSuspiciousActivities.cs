using BungieSharper.Client;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using BungieSharper.Entities.GroupsV2;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task<IEnumerable<SuspiciousContainer>> GetSuspiciousActivitiesAsync(DestinyActivityModeType? activityType = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var activities = await activitiesDB.GetSuspiciousActivitiesAsync((int?)activityType, DateTime.UtcNow.AddDays(-3));

            var users = await activitiesDB.GetUsersAsync();
            var userIDs = users.Select(x => x.UserID).ToHashSet();

            ConcurrentBag<SuspiciousContainer> suspiciousContainers = new();
            ConcurrentDictionary<long, SuspiciousUser> suspiciousUsers = new();

            var chunks = activities.Chunk(activities.Count() / 8 + 1);

            var tasks = chunks.Select(x => Task.Run(async () =>
            {
                foreach (var activity in x)
                {
                    var report = await apiClient.Api
                    .Destiny2_GetPostGameCarnageReport(activity.ActivityID);

                    List<SuspiciousUser> susUsers = new();

                    foreach (var entry in report.Entries.DistinctBy(x => x.Player.DestinyUserInfo.MembershipId))
                    {
                        var player = entry.Player;

                        if (!suspiciousUsers.ContainsKey(player.DestinyUserInfo.MembershipId))
                        {
                            if (userIDs.Contains(player.DestinyUserInfo.MembershipId))
                                suspiciousUsers.TryAdd(player.DestinyUserInfo.MembershipId, new SuspiciousUser
                                {
                                    IsClanMember = true,
                                    UserName = $"{player.DestinyUserInfo.BungieGlobalDisplayName}#{player.DestinyUserInfo.BungieGlobalDisplayNameCode}",
                                    ClanSign = "UA"
                                });
                            else
                            {
                                var groups = await apiClient.Api
                                .GroupV2_GetGroupsForMember(GroupsForMemberFilter.All, GroupType.Clan, player.DestinyUserInfo.MembershipId, player.DestinyUserInfo.MembershipType);

                                suspiciousUsers.TryAdd(player.DestinyUserInfo.MembershipId, new SuspiciousUser
                                {
                                    IsClanMember = false,
                                    UserName = $"{player.DestinyUserInfo.BungieGlobalDisplayName}#{player.DestinyUserInfo.BungieGlobalDisplayNameCode}",
                                    ClanSign = groups.Results.FirstOrDefault()?.Group.ClanInfo.ClanCallsign,
                                    ClanName = groups.Results.FirstOrDefault()?.Group.Name
                                });
                            }
                        }

                        susUsers.Add(suspiciousUsers[player.DestinyUserInfo.MembershipId]);
                    }

                    var teamScore = report.Entries.FirstOrDefault()?.Values["teamScore"]?.Basic.Value;

                    suspiciousContainers.Add(new SuspiciousContainer
                    {
                        Users = susUsers,
                        ActivityType = report.ActivityDetails.Mode,
                        Period = report.Period.ToLocalTime(),
                        Score = teamScore > 0 ? teamScore.ToString() : string.Empty,
                    });
                }
            }));

            await Task.WhenAll(tasks);

            return suspiciousContainers.OrderByDescending(x => x.Period);
        }
    }
}
