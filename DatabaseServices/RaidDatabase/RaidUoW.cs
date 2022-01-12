using Microsoft.EntityFrameworkCore;
using RaidDatabase.ORM;

namespace RaidDatabase
{
    public class RaidUoW : IRaidDB
    {
        private readonly RaidContext _context;

        public RaidUoW(RaidContext context) => _context = context;

        public IEnumerable<Raid> Raids => _context.Raids.Where(x => x.IsActive);

        public async Task<Raid> GetRaidAsync(ulong raidID) =>
            await _context.Raids.FindAsync(raidID);

        public async Task<Raid> GetRaidWithReservationsAsync(ulong raidID) =>
            await _context.Raids.Include(x => x.Reservations).FirstOrDefaultAsync(x => x.RaidID == raidID);

        public async Task AddRaidAsync(Raid raid)
        {
            raid.IsActive = true;

            await _context.Raids.AddAsync(raid);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRaidAsync(Raid raid)
        {
            _context.Raids.Update(raid);

            await _context.SaveChangesAsync();
        }

        public async Task DisableRaidAsync(ulong raidID)
        {
            var dbRaid = await GetRaidAsync(raidID);

            if (dbRaid is not null)
            {
                dbRaid.IsActive = false;

                _context.Raids.Update(dbRaid);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SubscribeUserAsync(ulong raidID, ulong userID)
        {
            var dbRaid = await GetRaidWithReservationsAsync(raidID);

            if (dbRaid is not null)
            {
                var reservation = dbRaid.Reservations.FirstOrDefault(x => x.RaidID == raidID && x.UserID == userID);

                if (reservation is null)
                {
                    var position = dbRaid.Reservations.Any() ?
                    dbRaid.Reservations.Max(x => x.Position) + 1 : 0;

                    await _context.Reservations.AddAsync(
                        new Reservation
                        {
                            RaidID = raidID,
                            UserID = userID,
                            Position = position
                        });

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task UnSubscribeUserAsync(ulong raidID, ulong userID)
        {
            var dbRaid = await GetRaidWithReservationsAsync(raidID);

            if (dbRaid is not null)
            {
                var reservation = dbRaid.Reservations.FirstOrDefault(x => x.RaidID == raidID && x.UserID == userID);

                if (reservation is not null)
                {
                    var otherReservations = dbRaid.Reservations.Where(x => x.Position > reservation.Position);

                    foreach (var r in otherReservations)
                        r.Position--;

                    _context.Remove(reservation);

                    _context.UpdateRange(otherReservations);

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
