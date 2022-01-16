namespace MusicService
{
    public interface IMusicPlayer
    {
        void Play();

        void Stop();

        void Pause();

        void Continue();

        void Next();

        void Previous();

        void Shuffle();
    }
}
