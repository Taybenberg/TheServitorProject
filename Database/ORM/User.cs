using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public record User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }

        public string UserName { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public DateTime ClanJoinDate { get; set; }

        public BungieNetApi.MembershipType MembershipType { get; set; }

        public ICollection<Character> Characters { get; set; }

        public ulong? DiscordUserID { get; set; }
    }
}
