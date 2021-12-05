using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace ServitorDiscordBot
{
    class MusicContainer
    {
        public class YoutubeVideo
        {
            public IVideo Video { get; init; }
            public Task<IStreamInfo> StreamInfo { get; init; }
        }

        YoutubeClient youtube = new();

        LinkedList<IVideo> videos = new();
        LinkedListNode<IVideo> currNode = null;

        private readonly object locker = new();

        private bool getPrev = false;

        public int Count
        {
            get
            {
                lock (locker)
                {
                    return videos.Count;
                }
            }
        }

        public int CurrIndex
        {
            get
            {
                lock (locker)
                {
                    return videos.ToList().IndexOf(currNode.Value);
                }
            }
        }

        public IVideo[] AllVideos
        {
            get
            {
                lock (locker)
                {
                    return videos.ToArray();
                }
            }
        }

        public YoutubeVideo CurrentYoutubeVideo
        {
            get
            {
                lock (locker)
                {
                    var video = currNode?.Value;

                    if (video is null)
                        return null;

                    return new YoutubeVideo
                    {
                        Video = video,
                        StreamInfo = Task.Run(async () =>
                        {
                            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Url);
                            return streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                        })
                    };
                }
            }
        }

        public YoutubeVideo NextYoutubeVideo
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

                    return CurrentYoutubeVideo;
                }
            }
        }

        private void AddVideo(IVideo video)
        {
            lock (locker)
            {
                videos.AddLast(video);
            }
        }

        public async Task AddAsync(string URL)
        {
            if (URL.Contains("list="))
            {
                foreach (var video in await youtube.Playlists.GetVideosAsync(URL))
                    AddVideo(video);
            }
            else
            {
                AddVideo(await youtube.Videos.GetAsync(URL));
            }

            lock (locker)
            {
                currNode ??= videos.First;
            }
        }

        public void Shuffle()
        {
            Random r = new Random();

            lock (locker)
            {
                var curr = new IVideo[] { currNode.Value };

                var shuffled = videos.Except(curr).OrderBy(x => r.Next());

                videos = new(curr.Concat(shuffled));

                currNode = videos.First;
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
