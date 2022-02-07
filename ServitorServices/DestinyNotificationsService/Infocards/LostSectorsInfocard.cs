namespace DestinyNotificationsService.Infocards
{
    public record LostSectorsInfocard
    {
        public DateTime ResetBegin { get; internal set; }

        public DateTime ResetEnd { get; internal set; }

        public string InfocardImageURL { get; internal set; }
    }
}
