using DataProcessor.DatabaseStats;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataProcessor
{
    public interface IStatsFactory
    {
        Task<ClanActivities> GetClanActivitiesAsync();

        Task<ClanStats> GetClanStatsAsync(string mode);

        Task<IEnumerable<string[]>> GetModesAsync();
    }
}
