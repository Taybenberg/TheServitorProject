using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.Xur
{
    public record XurInventory
    {
        [Key]
        public int WeeklyResetID { get; set; }

        public DateTime WeeklyResetBegin { get; set; }

        public DateTime WeeklyResetEnd { get; set; }

        public string? XurLocation { get; set; }

        public string InfocardImageURL { get; set; }

        public ICollection<XurItem> XurItems { get; set; }
    }
}
