using BumperDatabase.ORM;
using Microsoft.EntityFrameworkCore;

namespace BumperDatabase
{
    public class BumperUoW : IBumperDB
    {
        private readonly BumperContext _context;

        public BumperUoW(BumperContext context) => _context = context;

        public Bump LastBump => _context.Bumps.OrderByDescending(x => x.BumpTime).FirstOrDefault();

        public IEnumerable<ulong> PingableUserIDs => _context.Users.Where(x => x.IsPingable).Select(x => x.UserID);

        public async Task<IEnumerable<Bump>> GetLastBumpsAsync(DateTime afterDate) =>
            await _context.Bumps.Where(x => x.BumpTime > afterDate).ToListAsync();

        public async Task AddBumpAsync(DateTime bumpTime, ulong userID)
        {
            if (!_context.Users.Any(x => x.UserID == userID))
                await AddUser(userID, false);

            await _context.Bumps.AddAsync(
                new Bump
                {
                    BumpTime = bumpTime,
                    UserID = userID
                });

            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateUserAsync(ulong userID, bool isPingable)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserID == userID);

            if (user is null)
                await AddUser(userID, isPingable);
            else
                user.IsPingable = isPingable;

            await _context.SaveChangesAsync();
        }

        private async Task AddUser(ulong userID, bool isPingable) =>
            await _context.Users.AddAsync(
                new User
                {
                    UserID = userID,
                    IsPingable = isPingable
                });
    }
}
