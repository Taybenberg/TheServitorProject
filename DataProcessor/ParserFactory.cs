using BungieNetApi;
using DataProcessor.Parsers;
using DataProcessor.Parsers.Inventory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataProcessor
{
    public class ParserFactory : IParserFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ParserFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public IInventoryParser<EververseInventory> GetEververseParser(string seasonName, DateTime seasonStart, int weekNumber) =>
            new EververseParser(seasonName, seasonStart, weekNumber);

        public IInventoryParser<LostSectorsInventory> GetLostSectorsParser() =>
            new LostSectorsParser();

        public IInventoryParser<ResourcesInventory> GetResourcesParser() =>
            new ResourcesParser();

        public IInventoryParser<OsirisInventory> GetOsirisParser() =>
            new TrialsOfOsirisParser();

        public IInventoryParser<RoadmapInventory> GetRoadmapParser() =>
            new RoadmapParser();

        public IInventoryParser<XurInventory> GetXurParser(bool getLocation)
        {

            using var scope = _scopeFactory.CreateScope();

            IApiClient apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            return new XurParser(apiClient, getLocation);
        }
    }
}
