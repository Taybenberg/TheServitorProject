using BungieNetApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public record User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }

        public string UserName { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public DateTime ClanJoinDate { get; set; }

        public MembershipType MembershipType { get; set; }

        public ICollection<Character> Characters { get; set; }

        public ulong? DiscordUserID { get; set; }
    }
}
