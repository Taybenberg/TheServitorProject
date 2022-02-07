using DestinyInfocardsDatabase.ORM.LostSectors;
using DestinyInfocardsDatabase.ORM.Xur;

namespace DestinyInfocardsDatabase
{
    public interface IInfocardsDB
    {
        Task AddLostSectorAsync(LostSectorsDailyReset dailyReset);
        Task UpdateLostSectorAsync(LostSectorsDailyReset dailyReset);
        Task<LostSectorsDailyReset?> GetLostSectorAsync(DateTime dailyResetBegin, DateTime dailyResetEnd);

        Task AddXurInventoryAsync(XurInventory inventory);
        Task UpdateXurInventoryAsync(XurInventory inventory);
        Task<XurInventory?> GetXurInventoryAsync(DateTime weeklyResetBegin, DateTime weeklyResetEnd);
    }
}
