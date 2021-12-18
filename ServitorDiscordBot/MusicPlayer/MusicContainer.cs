using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;

namespace ServitorDiscordBot
{
    class MusicContainer
    {
        YoutubeClient youtube = new();

        LinkedList<IAudio> audios = new();
        LinkedListNode<IAudio> currNode = null;

        private readonly object locker = new();

        private bool getPrev = false;

        public int Count
        {
            get
            {
                lock (locker)
                {
                    return audios.Count;
                }
            }
        }

        public int CurrIndex
        {
            get
            {
                lock (locker)
                {
                    return audios.ToList().IndexOf(currNode.Value);
                }
            }
        }

        public IAudio[] AllAudios
        {
            get
            {
                lock (locker)
                {
                    return audios.ToArray();
                }
            }
        }

        public IAudio CurrentAudio
        {
            get
            {
                lock (locker)
                {
                    var audio = currNode?.Value;

                    if (audio is null)
                        return null;

                    return audio;
                }
            }
        }

        public IAudio NextAudio
        {
            get
            {
                lock (locker)
                {
                    if (getPrev)
                    {
                        getPrev = false;

                        currNode = currNode.Previous;
                    }
                    else
                    {
                        currNode = currNode.Next;
                    }

                    return CurrentAudio;
                }
            }
        }

        private void AddAudio(IAudio audio)
        {
            lock (locker)
            {
                audios.AddLast(audio);
            }
        }

        public async Task AddAsync(string URL)
        {
            if (URL.Contains("youtube.com") || URL.Contains("youtu.be"))
            {
                if (URL.Contains("?list="))
                {
                    var playlist = await youtube.Playlists.GetVideosAsync(URL);

                    foreach (var video in playlist)
                        AddAudio(new YouTubeVideo(video, youtube));
                }
                else
                {
                    var video = await youtube.Videos.GetAsync(URL);

                    AddAudio(new YouTubeVideo(video, youtube));
                }
            }
            else if (URL.Contains("soundcloud.com"))
            {
                var clientID = "xxDgkKDfvcceijWS9J5ZVxf7NZV6epqK";
                var widget = $"https://api-widget.soundcloud.com/resolve?url={URL}&format=json&client_id={clientID}";

                if (URL.Contains("/sets/"))
                {
                    var playlist = await JsonSerializer.DeserializeAsync<SoundCloud.Playlist>(await widget.GetStreamAsync());

                    foreach (var track in playlist.tracks)
                        AddAudio(new SoundCloudAudio(track, clientID));
                }
                else
                {
                    var track = await JsonSerializer.DeserializeAsync<SoundCloud.Track>(await widget.GetStreamAsync());

                    AddAudio(new SoundCloudAudio(track, clientID));
                }
            }

            lock (locker)
            {
                currNode ??= audios.First;
            }
        }

        public void Shuffle()
        {
            Random r = new Random();

            lock (locker)
            {
                var curr = new IAudio[] { currNode.Value };

                var shuffled = audios.Except(curr).OrderBy(x => r.Next());

                audios = new(curr.Concat(shuffled));

                currNode = audios.First;
            }
        }

        public void GetPreviousNext()
        {
            lock (locker)
            {
                getPrev = true;
            }
        }
    }
}
