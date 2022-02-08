using DestinyInfocardsDatabase.ORM.Eververse;
using DestinyInfocardsDatabase.ORM.LostSectors;
using DestinyInfocardsDatabase.ORM.Resources;
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

        Task AddVendorsInventoryAsync(VendorsDailyReset dailyReset);
        Task UpdateVendorsInventoryAsync(VendorsDailyReset dailyReset);
        Task<VendorsDailyReset?> GetVendorsInventoryAsync(DateTime dailyResetBegin, DateTime dailyResetEnd);

        Task AddEververseInventoryAsync(EververseInventory inventory);
        Task UpdateEververseInventoryAsync(EververseInventory inventory);
        Task<EververseInventory?> GetEververseInventoryAsync(DateTime weeklyResetBegin, DateTime weeklyResetEnd);
    }
}
