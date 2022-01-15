using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityDatabase.ORM
{
    public record Activity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong ActivityID { get; set; }

        public ulong ChannelID { get; set; }

        public DateTime PlannedDate { get; set; }

        public int ActivityType { get; set; }

        public string? ActivityName { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
