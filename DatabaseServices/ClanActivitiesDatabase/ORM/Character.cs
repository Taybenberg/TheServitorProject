using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanActivitiesDatabase.ORM
{
    public record Character
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CharacterID { get; set; }

        public long UserID { get; set; }
        public User User { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public int Class { get; set; }

        public int Race { get; set; }

        public int Gender { get; set; }

        public ICollection<ActivityUserStats> ActivityUserStats { get; set; }
    }
}
