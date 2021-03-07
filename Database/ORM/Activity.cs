using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public record Activity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ActivityID { get; set; }

        public DateTime Period { get; set; }

        public BungieNetApi.ActivityType ActivityType { get; set; }

        public ICollection<ActivityUserStats> ActivityUserStats { get; set; }

        public int? SuspicionIndex { get; set; }
    }
}
