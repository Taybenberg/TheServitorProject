using System.ComponentModel.DataAnnotations;

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
