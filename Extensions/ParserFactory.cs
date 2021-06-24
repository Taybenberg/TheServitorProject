using Extensions.Inventory;
using Extensions.Parsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Extensions
{
    public class ParserFactory
    {
        IServiceScopeFactory _scopeFactory;

        public ParserFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public IInventoryParser<EververseInventory> GetEververseParser(string seasonName, DateTime seasonStart, int weekNumber)
        {
            using var scope = _scopeFactory.CreateScope();

            return ActivatorUtilities.CreateInstance<EververseParser>(scope.ServiceProvider, seasonName, seasonStart, weekNumber);
        }

        public IInventoryParser<LostSectorsInventory> GetLostSectorsParser()
        {
            using var scope = _scopeFactory.CreateScope();

            return ActivatorUtilities.CreateInstance<LostSectorsParser>(scope.ServiceProvider);
        }

        public IInventoryParser<ResourcesInventory> GetResourcesParser()
        {
            using var scope = _scopeFactory.CreateScope();

            return ActivatorUtilities.CreateInstance<ResourcesParser>(scope.ServiceProvider);
        }

        public IInventoryParser<OsirisInventory> GetOsirisParser()
        {
            using var scope = _scopeFactory.CreateScope();

            return ActivatorUtilities.CreateInstance<TrialsOfOsirisParser>(scope.ServiceProvider);
        }

        public IInventoryParser<RoadmapInventory> GetRoadmapParser()
        {
            using var scope = _scopeFactory.CreateScope();

            return ActivatorUtilities.CreateInstance<RoadmapParser>(scope.ServiceProvider);
        }

        public IInventoryParser<XurInventory> GetXurParser(bool getLocation)
        {
            using var scope = _scopeFactory.CreateScope();

            return ActivatorUtilities.CreateInstance<XurParser>(scope.ServiceProvider, getLocation);
        }
    }
}
