using BungieNetApi.Enums;
using System;

namespace BungieNetApi.Entities
{
    public class Character
    {
        public long CharacterId { get; internal set; }

        public long MembershipId { get; internal set; }

        public DateTime DateLastPlayed { get; internal set; }

        public DestinyClass Class { get; internal set; }

        public DestinyRace Race { get; internal set; }

        public DestinyGender Gender { get; internal set; }
    }
}
