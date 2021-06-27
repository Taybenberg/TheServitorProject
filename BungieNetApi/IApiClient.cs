﻿using BungieNetApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BungieNetApi
{
    public interface IApiClient
    {
        Clan Clan { get; }

        Task<Milestone> GetMilestonesAsync();

        Task<IEnumerable<Item>> GetXurItemsAsync();
    }
}
