using System;

namespace BungieNetApi
{
    public record User
    {
        public long MembershipId { get; set; }

        public string LastSeenDisplayName { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public DateTime ClanJoinDate { get; set; }

        public Character[] Characters { get; set; }

        public MembershipType MembershipType { get; set; }
    }
}
