namespace BungieNetApi
{
    public record ActivityUserStats
    {
        public long CharacterId { get; set; }

        public long MembershipId { get; set; }

        public string DisplayName { get; set; }

        public float ActivityDurationSeconds { get; set; }

        public bool Completed { get; set; }

        public float CompletionReasonValue { get; set; }

        public string CompletionReasonDisplayValue { get; set; }

        public float StandingValue { get; set; }

        public string StandingDisplayValue { get; set; }

        public float Score { get; set; }

        public float TeamScore { get; set; }
    }
}
