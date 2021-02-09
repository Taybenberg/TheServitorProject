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
    public record Character
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CharacterID { get; set; }

        public long UserID { get; set; }
        public User User { get; set; }

        public DateTime DateLastPlayed { get; set; }

        public BungieNetApi.DestinyClass Class { get; set; }

        public BungieNetApi.DestinyRace Race { get; set; }

        public BungieNetApi.DestinyGender Gender { get; set; }

        public ICollection<ActivityUserStats> ActivityUserStats { get; set; }
    }
}
