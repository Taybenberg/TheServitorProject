using DataProcessor.DatabaseStats;
using System.Threading.Tasks;

namespace DataProcessor
{
    public interface IStatsFactory
    {
        Task<ClanActivitiesCounter> GetClanActivitiesCounterAsync();
    }
}
