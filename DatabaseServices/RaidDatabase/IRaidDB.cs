using RaidDatabase.ORM;

namespace RaidDatabase
{
    public interface IRaidDB
    {
        IEnumerable<Raid> Raids { get; }

        Task<Raid> GetRaidAsync(ulong raidID);

        Task<Raid> GetRaidWithReservationsAsync(ulong raidID);

        Task AddRaidAsync(Raid raid);

        Task UpdateRaidAsync(Raid raid);

        Task DisableRaidAsync(ulong raidID);

        Task SubscribeUserAsync(ulong raidID, ulong userID);

        Task UnSubscribeUserAsync(ulong raidID, ulong userId);
    }
}
