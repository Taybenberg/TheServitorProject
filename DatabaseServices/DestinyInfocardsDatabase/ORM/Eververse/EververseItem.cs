using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.Eververse
{
    public record EververseItem
    {
        [Key]
        public int ItemID { get; set; }

        public ItemCategory ItemCategory { get; set; }

        public string ItemIconURL { get; set; }

        public string? ItemSeasonIconURL { get; set; }

        public int EververseInventoryID { get; set; }
        public EververseInventory EververseInventory { get; set; }
    }
}
