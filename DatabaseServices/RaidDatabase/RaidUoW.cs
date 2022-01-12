namespace RaidDatabase
{
    public class RaidUoW : IRaidDB
    {
        private readonly RaidContext _context;

        public RaidUoW(RaidContext context) => _context = context;
    }
}
