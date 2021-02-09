using System;

namespace BungieNetApi
{
    public record Character
    {
        public long CharacterId { get; set; }

        public long MembershipId { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public DestinyClass Class { get; set; }

        public DestinyRace Race { get; set; }

        public DestinyGender Gender { get; set; }
    }
}
