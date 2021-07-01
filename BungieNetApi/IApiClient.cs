using BungieNetApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BungieNetApi
{
    public interface IApiClient
    {
        IEntityFactory EntityFactory { get; }

        Clan Clan { get; }

        Task<Milestone> GetMilestonesAsync();

        Task<IEnumerable<Item>> GetXurItemsAsync();
    }
}
