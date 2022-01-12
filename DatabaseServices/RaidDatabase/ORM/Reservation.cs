using System.ComponentModel.DataAnnotations;

namespace RaidDatabase.ORM
{
    public record Reservation
    {
        [Key]
        public long ReservationID { get; set; }

        public ulong RaidID { get; set; }
        public Raid Raid { get; set; }

        public int Position { get; set; }

        public ulong UserID { get; set; }
    }
}
