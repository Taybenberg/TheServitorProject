using System.Collections.Generic;

namespace DataProcessor.Inventory
{
    public record OsirisInventory
    {
        public string Location { get; set; }

        public List<List<string>> IconURLs { get; set; } = new();
    }
}
