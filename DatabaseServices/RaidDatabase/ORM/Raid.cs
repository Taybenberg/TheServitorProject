using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaidDatabase.ORM
{
    public record Raid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong RaidID { get; set; }

        public DateTime PlannedDate { get; set; }

        public int RaidType { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
