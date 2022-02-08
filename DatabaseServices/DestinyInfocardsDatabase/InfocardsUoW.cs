using DestinyInfocardsDatabase.ORM.Eververse;
using DestinyInfocardsDatabase.ORM.LostSectors;
using DestinyInfocardsDatabase.ORM.Resources;
using DestinyInfocardsDatabase.ORM.Xur;
using Microsoft.EntityFrameworkCore;

namespace DestinyInfocardsDatabase
{
    public class InfocardsUoW : IInfocardsDB
    {
        private readonly InfocardsContext _context;

        public InfocardsUoW(InfocardsContext context) => _context = context;

        public async Task AddLostSectorAsync(LostSectorsDailyReset dailyReset)
        {
            await _context.LostSectorsDailyResets.AddAsync(dailyReset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLostSectorAsync(LostSectorsDailyReset dailyReset)
        {
            _context.LostSectorsDailyResets.Update(dailyReset);
            await _context.SaveChangesAsync();
        }

        public async Task<LostSectorsDailyReset?> GetLostSectorAsync(DateTime dailyResetBegin, DateTime dailyResetEnd) =>
            await _context.LostSectorsDailyResets
                .Include(x => x.LostSectors)
                .FirstOrDefaultAsync(x => x.DailyResetBegin == dailyResetBegin && x.DailyResetEnd == dailyResetEnd);

        public async Task AddXurInventoryAsync(XurInventory inventory)
        {
            await _context.XurInventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateXurInventoryAsync(XurInventory inventory)
        {
            _context.XurInventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<XurInventory?> GetXurInventoryAsync(DateTime weeklyResetBegin, DateTime weeklyResetEnd) =>
            await _context.XurInventories
                .Include(x => x.XurItems)
                .FirstOrDefaultAsync(x => x.WeeklyResetBegin == weeklyResetBegin && x.WeeklyResetEnd == weeklyResetEnd);

        public async Task AddVendorsInventoryAsync(VendorsDailyReset dailyReset)
        {
            await _context.VendorsDailyResets.AddAsync(dailyReset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVendorsInventoryAsync(VendorsDailyReset dailyReset)
        {
            _context.VendorsDailyResets.Update(dailyReset);
            await _context.SaveChangesAsync();
        }

        public async Task<VendorsDailyReset?> GetVendorsInventoryAsync(DateTime dailyResetBegin, DateTime dailyResetEnd) =>
            await _context.VendorsDailyResets
                .Include(x => x.ResourceItems)
                .FirstOrDefaultAsync(x => x.DailyResetBegin == dailyResetBegin && x.DailyResetEnd == dailyResetEnd);

        public async Task AddEververseInventoryAsync(EververseInventory inventory)
        {
            await _context.EververseInventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEververseInventoryAsync(EververseInventory inventory)
        {
            _context.EververseInventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<EververseInventory?> GetEververseInventoryAsync(DateTime weeklyResetBegin, DateTime weeklyResetEnd) =>
             await _context.EververseInventories
                .Include(x => x.EververseItems)
                .FirstOrDefaultAsync(x => x.WeeklyResetBegin == weeklyResetBegin && x.WeeklyResetEnd == weeklyResetEnd);
    }
}
