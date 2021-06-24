using System;
using System.Collections.Generic;

namespace Extensions.Inventory
{
    public record ResourcesInventory
    {
        public DateTime ResetBegin { get; set; }

        public DateTime ResetEnd { get; set; }

        public List<ResourceItem> SpiderResources { get; set; } = new();

        public List<ResourceItem> Banshee44Resources { get; set; } = new();

        public List<ResourceItem> Ada1Resources { get; set; } = new();
    }

    public record ResourceItem
    {
        public string ResourceName { get; set; }

        public string ResourceIconURL { get; set; }

        public string ResourceCurrencyQuantity { get; set; }

        public string ResourceCurrencyIconURL { get; set; }
    }
}
