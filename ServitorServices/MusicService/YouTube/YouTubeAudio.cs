using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MusicService.YouTube
{
    public class YouTubeAudio : IAudio
    {
        private readonly IVideo _video;

        public YouTubeAudio(IVideo video) => _video = video;

        public string Title => _video.Title;

        public TimeSpan Duration => _video.Duration.Value;

        public string CoverURL => _video.Thumbnails.FirstOrDefault()?.Url;

        public Task<string> URL =>
        Task.Run(async () =>
        {
            var streamManifest = await new YoutubeClient().Videos.Streams.GetManifestAsync(_video.Url);

            var streams = streamManifest.GetAudioOnlyStreams();

            var opusStream = streams.Where(x => x.AudioCodec == "opus").TryGetWithHighestBitrate();

            return opusStream?.Url ?? streams.GetWithHighestBitrate().Url;
        });

        public static async Task<IEnumerable<IAudio>> GetAsync(string url)
        {
            var client = new YoutubeClient();

            if (url.Contains("?list="))
            {
                var playlist = await client.Playlists.GetVideosAsync(url);

                return playlist.Select(x => new YouTubeAudio(x));
            }
            else
            {
                var video = await client.Videos.GetAsync(url);

                return new YouTubeAudio[] { new YouTubeAudio(video) };
            }
        }
    }
}
