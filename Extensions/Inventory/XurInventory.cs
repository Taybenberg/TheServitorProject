using System.Collections.Generic;

namespace Extensions.Inventory
{
    public record XurInventory
    {
        public string Location { get; set; }

        public List<XurItem> XurItems { get; set; } = new();
    }

    public record XurItem
    {
        public string ItemName { get; set; }

        public string ItemIconURL { get; set; }

        public string ItemClass { get; set; }
    }
}
