using DestinyInfocardsDatabase.ORM.LostSectors;
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

        public Task<LostSectorsDailyReset?> GetLostSectorAsync(DateTime dailyResetBegin, DateTime dailyResetEnd) =>
            _context.LostSectorsDailyResets
                .Include(x => x.LostSectors)
                .FirstOrDefaultAsync(x => x.DailyResetBegin == dailyResetBegin && x.DailyResetEnd == dailyResetEnd);
    }
}
