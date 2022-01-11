using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BumperDatabase.ORM
{
    public record User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong UserID { get; set; }

        public bool IsPingable { get; set; }

        public ICollection<Bump> Bumps { get; set; }
    }
}
