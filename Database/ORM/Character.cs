using BungieNetApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.ORM
{
    public record Character
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CharacterID { get; set; }

        public long UserID { get; set; }
        public User User { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public DestinyClass Class { get; set; }

        public DestinyRace Race { get; set; }

        public DestinyGender Gender { get; set; }

        public ICollection<ActivityUserStats> ActivityUserStats { get; set; }
    }
}
