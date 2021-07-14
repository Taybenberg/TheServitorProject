using BungieNetApi;
using Database;
using DataProcessor.Localization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseStats
{
    public class SuspiciousActivities : IStats
    {
        private bool _isNightfallsOnly;

        private readonly IApiClient _apiClient;

        private readonly IClanDB _clanDB;

        internal SuspiciousActivities(IClanDB clanDB, IApiClient apiClient, bool isNightfallsOnly) =>
            (_clanDB, _apiClient, _isNightfallsOnly) = (clanDB, apiClient, isNightfallsOnly);

        public async Task InitAsync()
        {
            
        }
    }
}
