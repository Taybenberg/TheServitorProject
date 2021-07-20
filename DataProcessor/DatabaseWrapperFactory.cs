using BungieNetApi;
using Database;
using DataProcessor.DatabaseWrapper;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class DatabaseWrapperFactory : IDatabaseWrapperFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseWrapperFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

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

        public async Task<FindUserByName> GetUserWithSimilarUserNameAsync(ulong discordUserID, string discordUserName)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            var user = new FindUserByName(clanDB, discordUserID, discordUserName);

            await user.InitAsync();

            return user;
        }

        public async Task<bool> RegisterUserAsync(long userID, ulong discordUserID)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            return await clanDB.RegisterUserAsync(userID, discordUserID);
        }

        public async Task<MyRaids> GetMyRaidsAsync(ulong discordUserID, DateTime seasonStart, int weekNumber)
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            var raids = new MyRaids(clanDB, discordUserID, seasonStart, weekNumber);

            await raids.InitAsync();

            return raids;
        }
    }
}
