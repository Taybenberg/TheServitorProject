using Flurl.Http;
using System.Text.Json;

namespace MusicService.SoundCloud
{
    public class SoundCloudAudio : IAudio
    {
        private readonly Track _track;
        private readonly string _clientID;

        public SoundCloudAudio(Track track, string clientID) =>
            (_track, _clientID) = (track, clientID);

        public string Title => 
            _track.publisher_metadata?.artist is not null ?
            $"{_track.publisher_metadata?.artist} - {_track.title}" : _track.title;

        public TimeSpan Duration => TimeSpan.FromMilliseconds(_track.duration);

        public string CoverURL => _track.artwork_url;

        public Task<string> URL =>
        Task.Run(async () =>
        {
            var link = $"{_track.media?.transcodings[^1].url}?client_id={_clientID}&track_authorization={_track.track_authorization}";

            var songUrl = await JsonSerializer.DeserializeAsync<SongUrl>(await link.GetStreamAsync());

            return songUrl.url;
        });

        public static async Task<IEnumerable<IAudio>> GetAsync(string url, string clientID)
        {
            var widget = $"https://api-widget.soundcloud.com/resolve?format=json&client_id={clientID}&url={url}";

            if (url.Contains("/sets/"))
            {
                var playlist = await JsonSerializer.DeserializeAsync<Playlist>(await widget.GetStreamAsync());

                return playlist.tracks.Select(x => new SoundCloudAudio(x, clientID));
            }
            else
            {
                var track = await JsonSerializer.DeserializeAsync<Track>(await widget.GetStreamAsync());

                return new SoundCloudAudio[] { new SoundCloudAudio(track, clientID) };
            }
        }
    }
}
