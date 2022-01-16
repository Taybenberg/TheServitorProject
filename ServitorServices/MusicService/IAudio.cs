namespace MusicService
{
    public interface IAudio
    {
        string Title { get; }

        TimeSpan Duration { get; }

        string CoverURL { get; }

        Task<string> URL { get; }
    }
}
