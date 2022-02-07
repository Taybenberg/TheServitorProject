using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.LostSectors
{
    public record LostSectorsDailyReset
    {
        [Key]
        public int DailyResetID { get; set; }

        public DateTime DailyResetBegin { get; set; }

        public DateTime DailyResetEnd { get; set; }

        public string InfocardImageURL { get; set; }

        public ICollection<LostSector> LostSectors { get; set; }
    }
}
