namespace DestinyInfocardsService.Infocards
{
    public record WeeklyMilestoneInfocard
    {
        public int WeekNumber { get; internal set; }

        public DateTime ResetBegin { get; internal set; }

        public DateTime ResetEnd { get; internal set; }

        public string InfocardImageURL { get; internal set; }
    }
}
