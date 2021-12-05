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

        LinkedList<YoutubeVideo> videos = new();
        LinkedListNode<YoutubeVideo> currNode = null;

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
                    return videos.Select(x => x.Video).ToArray();
                }
            }
        }

        public YoutubeVideo CurrentYoutubeVideo
        {
            get
            {
                lock (locker)
                {
                    return currNode?.Value;
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

                    return currNode?.Value;
                }
            }
        }

        private void AddVideo(IVideo video)
        {
            lock (locker)
            {
                videos.AddLast(new YoutubeVideo
                {
                    Video = video,
                    StreamInfo = Task.Run(async () =>
                    {
                        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Url);
                        return streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    })
                });
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
                var curr = new YoutubeVideo[] { currNode.Value };

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
