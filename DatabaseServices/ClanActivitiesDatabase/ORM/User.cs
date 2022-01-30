using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClanActivitiesDatabase.ORM
{
    public record User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }

        public long ClanID { get; set; }

        public string UserName { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public DateTime ClanJoinDate { get; set; }

        public int MembershipType { get; set; }

        public ICollection<Character> Characters { get; set; }

        public ulong? DiscordUserID { get; set; }
    }
}
