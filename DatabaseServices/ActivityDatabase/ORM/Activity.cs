using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityDatabase.ORM
{
    public record Activity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong ActivityID { get; set; }

        public DateTime PlannedDate { get; set; }

        public string ActivityType { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
