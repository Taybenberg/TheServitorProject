﻿using DataProcessor.Inventory;
using DataProcessor.Parsers;
using System;

namespace DataProcessor
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