using Flurl.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace ServitorDiscordBot
{
    interface IAudio
    {
        public string Title { get; }
        public string Duration { get; }
        public Task<string> URL { get; }
    }

    public class YouTubeVideo : IAudio
    {
        private readonly IVideo _video;
        private readonly YoutubeClient _youtube;

        public YouTubeVideo(IVideo video, YoutubeClient client)
        {
            _video = video;
            _youtube = client;
        }

        public string Title => _video.Title;

        public string Duration
        { 
            get 
            {
                var span = (TimeSpan)_video.Duration;

                if (span.Hours > 0)
                    return span.ToString(@"hh\:mm\:ss");

                return span.ToString(@"mm\:ss");
            } 
        }

        public Task<string> URL =>
        Task.Run(async () =>
        {
            var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(_video.Url);
            return streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate().Url;
        });
    }

    public class SoundCloudAudio : IAudio
    {
        private readonly SoundCloud.Track _track;
        private readonly string _clientID;

        public SoundCloudAudio(SoundCloud.Track track, string clientID)
        {
            _track = track;
            _clientID = clientID;
        }

        public string Title => _track.publisher_metadata?.artist is not null ?
            $"{_track.publisher_metadata?.artist} - {_track.title}" : _track.title;

        public string Duration
        {
            get
            {
                var span = TimeSpan.FromMilliseconds(_track.duration);

                if (span.Hours > 0)
                    return span.ToString(@"hh\:mm\:ss");

                return span.ToString(@"mm\:ss");
            }
        }

        public Task<string> URL =>
        Task.Run(async () =>
        {
            var link = $"{_track.media.transcodings[^1].url}?client_id={_clientID}&track_authorization={_track.track_authorization}";

            var songUrl = await JsonSerializer.DeserializeAsync<SoundCloud.SongUrl>(await link.GetStreamAsync());

            return songUrl.url;
        });
    }
}
