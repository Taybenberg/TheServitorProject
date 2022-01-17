namespace MusicService
{
    public interface IMusicPlayer
    {
        event Func<ulong?, Task> OnUpdate;

        bool IsPlaying { get; }

        IEnumerable<(bool isCurrent, IAudio audio)> Queue { get; }

        bool Init();

        void Stop();

        void Pause();

        void Continue();

        void Next();

        void Previous();

        void Shuffle();

        Task AddAsync(string URL);

        Task PlayAsync(string url, Stream audioOutStream, ulong instanceID);

        Task PlayDirectAsync(string URL, Stream audioOutStream);
    }
}
