using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.Xur
{
    public record XurItem
    {
        [Key]
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        public string ItemIconURL { get; set; }

        public string ItemClass { get; set; }

        public int XurInventoryID { get; set; }
        public XurInventory XurInventory { get; set; }
    }
}
