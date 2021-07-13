using Database;
using DataProcessor.DatabaseStats;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class StatsFactory : IStatsFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StatsFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<ClanActivitiesCounter> GetClanActivitiesCounterAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            IClanDB clanDB = scope.ServiceProvider.GetRequiredService<IClanDB>();

            var counter = new ClanActivitiesCounter(clanDB);

            await counter.InitAsync();

            return counter;
        }
    }
}
