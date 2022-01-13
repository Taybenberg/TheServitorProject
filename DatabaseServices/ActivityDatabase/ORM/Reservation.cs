using System.ComponentModel.DataAnnotations;

namespace ActivityDatabase.ORM
{
    public record Reservation
    {
        [Key]
        public long ReservationID { get; set; }

        public ulong ActivityID { get; set; }
        public Activity Activity { get; set; }

        public int Position { get; set; }

        public ulong UserID { get; set; }
    }
}
