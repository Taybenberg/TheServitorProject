using BumperDatabase.ORM;

namespace BumperDatabase
{
    public interface IBumperDB
    {
        Bump LastBump { get; }

        IEnumerable<ulong> PingableUserIDs { get; }

        Task AddBumpAsync(DateTime dateTime, ulong userID);

        Task AddOrUpdateUserAsync(ulong userID, bool isPingable);

        Task<IEnumerable<Bump>> GetLastBumpsAsync(DateTime afterDate);
    }
}
