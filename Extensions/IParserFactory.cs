using Extensions.Inventory;
using Extensions.Parsers;
using System;

namespace Extensions
{
    public interface IParserFactory
    {
        IInventoryParser<EververseInventory> GetEververseParser(string seasonName, DateTime seasonStart, int weekNumber);

        IInventoryParser<LostSectorsInventory> GetLostSectorsParser();

        IInventoryParser<ResourcesInventory> GetResourcesParser();

        IInventoryParser<OsirisInventory> GetOsirisParser();

        IInventoryParser<RoadmapInventory> GetRoadmapParser();

        IInventoryParser<XurInventory> GetXurParser(bool getLocation);
    }
}
