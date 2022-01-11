using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BumperDatabase.ORM
{
    public record Bump
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime BumpTime { get; set; }

        public ulong UserID { get; set; }
        public User User { get; set; }
    }
}
