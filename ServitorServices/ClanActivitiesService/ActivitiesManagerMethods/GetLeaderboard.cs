using BungieSharper.Client;
using BungieSharper.Entities.Destiny;
using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task<LeaderboardContainer> GetLeaderboardAsync(ulong userID, DestinyActivityModeType activityType)
        {
            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var user = await activitiesDB.GetUserByDiscordIdAsync(userID);

            if (user is null)
                return null;

            var mode = ((int)activityType).ToString();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var statIDs = CommonData.Localization.Translation.StatNames.Keys;

            var chunks = statIDs.Chunk(statIDs.Count() / 8 + 1);

            ConcurrentBag<LeaderboardStat> stats = new();

            var tasks = chunks.Select(x => Task.Run(async () =>
            {
                foreach (var statID in x)
                {
                    try
                    {
                        var leaderboard = await apiClient.Api.Destiny2_GetClanLeaderboards(user.ClanID, 100, mode, statID);

                        var entries = leaderboard.First().Value.First().Value.Entries;

                        var userEntry = entries.First(y => y.Player.DestinyUserInfo.MembershipId == user.UserID);

                        var leaderboardEntries = new List<LeaderboardEntry>()
                        {
                            new LeaderboardEntry
                            {
                                IsCurrUser = true,
                                Rank = userEntry.Rank,
                                Value = userEntry.Value.Basic.DisplayValue,
                                UserName = $"{userEntry.Player.DestinyUserInfo.BungieGlobalDisplayName}#{userEntry.Player.DestinyUserInfo.BungieGlobalDisplayNameCode}",
                                DestinyClass = Enum.Parse<DestinyClass>(userEntry.Player.CharacterClass)
                            }
                        };

                        leaderboardEntries.AddRange(entries
                            .Take(3)
                            .Where(y => y.Rank != userEntry.Rank)
                            .Select(y => new LeaderboardEntry
                            {
                                IsCurrUser = false,
                                Rank = y.Rank,
                                Value = y.Value.Basic.DisplayValue,
                                UserName = $"{y.Player.DestinyUserInfo.BungieGlobalDisplayName}#{y.Player.DestinyUserInfo.BungieGlobalDisplayNameCode}",
                                DestinyClass = Enum.Parse<DestinyClass>(y.Player.CharacterClass)
                            }));

                        stats.Add(new LeaderboardStat
                        {
                            StatName = CommonData.Localization.Translation.StatNames[statID],
                            Leaders = leaderboardEntries.OrderBy(y => y.Rank)
                        });
                    }
                    catch { }
                }
            }));

            await Task.WhenAll(tasks);

            return new LeaderboardContainer
            {
                LeaderboardStats = stats.OrderBy(x => x.StatName)
            };
        }
    }
}
