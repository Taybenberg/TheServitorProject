using System.Collections.Generic;

namespace DataProcessor.Inventory
{
    public record EververseInventory
    {
        public string Week { get; set; }

        public string Season { get; set; }

        public string SeasonIconURL { get; set; }

        public List<List<EververseItem>> EververseItems { get; set; } = new();
    }

    public record EververseItem
    {
        public string Icon1URL { get; set; }

        public string Icon2URL { get; set; }
    }
}
