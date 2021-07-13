using BungieNetApi;
using Database;
using DataProcessor.DatabaseStats;
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

            var counter = new ClanActivities(clanDB);

            await counter.InitAsync();

            return counter;
        }

        public async Task<ClanStats> GetClanStatsAsync(string mode)
        {
            using var scope = _scopeFactory.CreateScope();

            IApiClient apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var stats = new ClanStats(apiClient, mode);

            await stats.InitAsync();

            return stats;
        }

        public async Task<IEnumerable<string[]>> GetModesAsync()
        {
            return TranslationDictionaries.StatsActivityNames.Values.OrderBy(x => x[0]);
        }
    }
}
