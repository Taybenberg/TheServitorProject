using BungieNetApi;
using Database;
using DataProcessor.Localization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseStats
{
    public class WeeklyMilestone : IStats
    {
        private readonly IApiClient _apiClient;

        internal WeeklyMilestone(IApiClient apiClient) => _apiClient = apiClient;

        public async Task InitAsync()
        {
            
        }
    }
}
