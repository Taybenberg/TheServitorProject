using System;
using System.Collections.Generic;

namespace Extensions.Inventory
{
    public record LostSectorsInventory
    {
        public DateTime ResetBegin { get; set; }

        public DateTime ResetEnd { get; set; }

        public List<LostSector> LostSectors { get; set; } = new();
    }

    public record LostSector
    {
        public int LightLevel { get; set; }

        public string SectorIconURL { get; set; }

        public string SectorName { get; set; }

        public string SectorReward { get; set; }
    }
}
