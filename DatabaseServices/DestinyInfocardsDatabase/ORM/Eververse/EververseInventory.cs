using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.Eververse
{
    public record EververseInventory
    {
        [Key]
        public int WeeklyResetID { get; set; }

        public int SeasonNumber { get; set; }

        public DateTime WeeklyResetBegin { get; set; }

        public DateTime WeeklyResetEnd { get; set; }

        public string InfocardImageURL { get; set; }

        public ICollection<EververseItem> EververseItems { get; set; }
    }
}
