namespace MusicService
{
    internal class MusicContainer
    {
        private LinkedList<IAudio> audios = new();
        private LinkedListNode<IAudio> currNode = null;

        private readonly string _soundcloudClientID;

        public MusicContainer(string soundcloudClientID) => 
            (_soundcloudClientID) = (soundcloudClientID);

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

        public int CurrentIndex => Array.IndexOf(AllAudios, currNode.Value);

        public IAudio CurrentAudio
        {
            get
            {
                lock (locker)
                {
                    return currNode?.Value;
                }
            }
        }

        public IAudio NextQueuedAudio
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

        public async Task AddAudioAsync(string url)
        {
            foreach (var audio in await GetAudiosAsync(url))
                AddAudio(audio);

            lock (locker)
            {
                currNode ??= audios.First;
            }
        }

        private async Task<IEnumerable<IAudio>> GetAudiosAsync(string url)
        {
            if (url.Contains("youtube.com") || url.Contains("youtu.be"))
                return await YouTube.YouTubeAudio.GetAsync(url);
            else if (url.Contains("soundcloud.com"))
                return await SoundCloud.SoundCloudAudio.GetAsync(url, _soundcloudClientID);
            return new IAudio[0];
        }

        private void AddAudio(IAudio audio)
        {
            lock (locker)
            {
                audios.AddLast(audio);
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
