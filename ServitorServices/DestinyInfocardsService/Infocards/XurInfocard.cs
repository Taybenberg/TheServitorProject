namespace DestinyInfocardsService.Infocards
{
    public record XurInfocard
    {
        public int WeekNumber { get; internal set; }

        public string XurLocation { get; internal set; }

        public string InfocardImageURL { get; internal set; }
    }
}
