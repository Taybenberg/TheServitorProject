namespace DestinyInfocardsDatabase
{
    public class InfocardsUoW : IInfocardsDB
    {
        private readonly InfocardsContext _context;

        public InfocardsUoW(InfocardsContext context) => _context = context;
    }
}
