using BungieNetApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BungieNetApi
{
    public interface IApiClient
    {
        IEntityFactory EntityFactory { get; }

        Clan Clan { get; }

        Task<Nightfall> GetNightfallAsync(long activityHash);

        Task<Milestone> GetMilestonesAsync();

        Task<IEnumerable<Item>> GetXurItemsAsync();
    }
}
