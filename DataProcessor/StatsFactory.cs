using BungieNetApi;
using Database;
using DataProcessor.DatabaseStats;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class StatsFactory : IStatsFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StatsFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<ClanActivities> GetClanActivitiesAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            var activities = new ClanActivities(clanDB);

            await activities.InitAsync();

            return activities;
        }

        public async Task<ClanStats> GetClanStatsAsync(string mode)
        {
            using var scope = _scopeFactory.CreateScope();

            IApiClient apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var stats = new ClanStats(apiClient, mode);

            await stats.InitAsync();

            return stats;
        }

        public async Task<IEnumerable<(string, string[])>> GetModesAsync()
        {
            return TranslationDictionaries.StatsActivityNames
                .OrderBy(x => x.Value[0])
                .Select(y => (EmojiContainer.GetActivityEmoji(y.Key), y.Value));
        }

        public async Task<Leaderboard> GetLeaderboardAsync(string mode, ulong discordUserID)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            IApiClient apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var leaderboard = new Leaderboard(clanDB, apiClient, mode, discordUserID);

            await leaderboard.InitAsync();

            return leaderboard;
        }

        public async Task<MyActivities> GetMyActivitiesAsync(ulong discordUserID)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            var activities = new MyActivities(clanDB, discordUserID);

            await activities.InitAsync();

            return activities;
        }

        public async Task<MyPartners> GetMyPartnersAsync(ulong discordUserID)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            var activities = new MyPartners(clanDB, discordUserID);

            await activities.InitAsync();

            return activities;
        }

        public async Task<SuspiciousActivities> GetSuspiciousActivitiesAsync(bool isNightfallsOnly)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            IApiClient apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var activities = new SuspiciousActivities(clanDB, apiClient, isNightfallsOnly);

            await activities.InitAsync();

            return activities;
        }

        public async Task<WeeklyMilestone> GetWeeklyMilestoneAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            IApiClient apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var milestone = new WeeklyMilestone(apiClient);

            await milestone.InitAsync();

            return milestone;
        }
    }
}
