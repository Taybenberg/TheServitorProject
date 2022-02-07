using System.ComponentModel.DataAnnotations;

namespace DestinyInfocardsDatabase.ORM.LostSectors
{
    public record LostSector
    {
        [Key]
        public int LostSectorID { get; set; }

        public string Name { get; set; }

        public string Reward { get; set; }

        public string LightLevel { get; set; }

        public string ImageURL { get; set; }

        public int LostSectorsDailyResetID { get; set; }
        LostSectorsDailyReset DailyReset { get; set; }
    }
}
