using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.Resources
{
    public record VendorsDailyReset
    {

        [Key]
        public int DailyResetID { get; set; }

        public int SeasonNumber { get; set; }

        public DateTime DailyResetBegin { get; set; }

        public DateTime DailyResetEnd { get; set; }

        public string InfocardImageURL { get; set; }

        public ICollection<ResourceItem> ResourceItems { get; set; }
    }
}
