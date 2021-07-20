using BungieNetApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.ORM
{
    public record Activity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ActivityID { get; set; }

        public DateTime Period { get; set; }

        public ActivityType ActivityType { get; set; }

        public ICollection<ActivityUserStats> ActivityUserStats { get; set; }

        public int? SuspicionIndex { get; set; }

        public long ReferenceHash { get; set; }

        public long ActivityHash { get; set; }
    }
}
