using DestinyInfocardsDatabase.ORM.LostSectors;

namespace DestinyInfocardsDatabase
{
    public interface IInfocardsDB
    {
        Task AddLostSectorAsync(LostSectorsDailyReset dailyReset);
        Task UpdateLostSectorAsync(LostSectorsDailyReset dailyReset);
        Task<LostSectorsDailyReset?> GetLostSectorAsync(DateTime dailyResetBegin, DateTime dailyResetEnd);
    }
}
