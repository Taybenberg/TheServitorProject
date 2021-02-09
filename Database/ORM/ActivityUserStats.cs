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
    public record ActivityUserStats
    {
        [Key]
        public long ActivityUserStatsID { get; set; }

        public long ActivityID { get; set; }
        public Activity Activity { get; set; }

        public long CharacterID { get; set; }
        public Character Character { get; set; }

        public float ActivityDurationSeconds { get; set; }

        public bool Completed { get; set; }

        public float CompletionReasonValue { get; set; }

        public string CompletionReasonDisplayValue { get; set; }

        public float StandingValue { get; set; }

        public string StandingDisplayValue { get; set; }
    }
}
