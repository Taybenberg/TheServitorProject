namespace MusicService
{
    public static class MusicExtensions
    {
        public static string GetAudioDuration(this TimeSpan duration) =>
            duration.TotalHours > 1.0 ? duration.ToString(@"hh\:mm\:ss") : duration.ToString(@"mm\:ss");
    }
}
