using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.Resources
{
    public record ResourceItem
    {
        [Key]
        public int ItemID { get; set; }

        public DestinyVendor DestinyVendor { get; set; }

        public string ResourceName { get; set; }

        public string ResourceIconURL { get; set; }

        public string ResourceCurrencyQuantity { get; set; }

        public string ResourceCurrencyIconURL { get; set; }

        public int VendorsDailyResetID { get; set; }
        public VendorsDailyReset DailyReset { get; set; }
    }
}
