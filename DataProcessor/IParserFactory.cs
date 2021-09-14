using DataProcessor.DatabaseWrapper;
using DataProcessor.Parsers;
using DataProcessor.Parsers.Inventory;
using System;

namespace DataProcessor
{
    public interface IParserFactory
    {
        IInventoryParser<(LostSectorsInventory, ResourcesInventory)> GetDailyResetParser(string seasonName, int weekNumber);

        IInventoryParser<(EververseInventory, WeeklyMilestone)> GetWeeklyResetParser(string seasonName, DateTime seasonStart, int weekNumber);

        IInventoryParser<EververseInventory> GetEververseParser(string seasonName, DateTime seasonStart, int weekNumber);

        IInventoryParser<LostSectorsInventory> GetLostSectorsParser();

        IInventoryParser<ResourcesInventory> GetResourcesParser();

        IInventoryParser<XurInventory> GetXurParser(bool getLocation);
    }
}
