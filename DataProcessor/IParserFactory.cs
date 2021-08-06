using DataProcessor.Parsers;
using DataProcessor.Parsers.Inventory;
using System;

namespace DataProcessor
{
    public interface IParserFactory
    {
        IInventoryParser<(LostSectorsInventory, ResourcesInventory)> GetDailyResetParser(string seasonName, int weekNumber);

        IInventoryParser<EververseInventory> GetEververseParser(string seasonName, DateTime seasonStart, int weekNumber);

        IInventoryParser<LostSectorsInventory> GetLostSectorsParser();

        IInventoryParser<ResourcesInventory> GetResourcesParser();

        IInventoryParser<OsirisInventory> GetOsirisParser();

        IInventoryParser<RoadmapInventory> GetRoadmapParser();

        IInventoryParser<XurInventory> GetXurParser(bool getLocation);
    }
}
