using Discord;
using Discord.Audio;
using ManagedBass;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace ServitorDiscordBot
{
    class MusicPlayer : IDisposable
    {
        private readonly ILogger<ServitorBot> _logger;

        public MusicPlayer(ILogger<ServitorBot> logger)
        {
            _logger = logger;

            Bass.Init();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Bass.PluginLoad("bassopus.dll");
                Bass.PluginLoad("basswebm.dll");
            }
            else
            {
                Bass.PluginLoad("libbassopus.so");
                Bass.PluginLoad("libbasswebm.so");
            }
        }

        public void Dispose() => Bass.Free();

        private readonly object locker = new();
        private bool isReserved = false;
        private bool skip = false;

        public bool TryReserve()
        {
            lock (locker)
            {
                if (!isReserved)
                {
                    isReserved = true;
                    skip = false;

                    return true;
                }

                return false;
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                isReserved = false;
                isPlaying = true;
            }
        }

        public void Next()
        {
            lock (locker)
            {
                if (isReserved)
                    skip = true;

                isPlaying = true;
            }
        }

        public void Prev()
        {
            musicContainer?.GetPreviousNext();

            lock (locker)
            {
                if (isReserved)
                    skip = true;

                isPlaying = true;
            }
        }

        public void Shuffle()
        {
            lock (locker)
            {
                if (!isReserved)
                    return;
            }

            musicContainer?.Shuffle();
        }

        public async Task GetQueueAsync(IMessageChannel channel)
        {
            lock (locker)
            {
                if (!isReserved)
                    return;
            }

            var curr = musicContainer?.CurrIndex;
            var videos = musicContainer?.AllVideos;

            if (curr is not null && videos is not null)
            {
                int count = videos.Length;

                string str = $"У черзі {count} відео:";

                for (int i = 0; i < count; i++)
                {
                    if (i == curr)
                        str += $"\n**{i + 1})** [{videos[i].Duration}] ***{videos[i].Title}***";
                    else
                        str += $"\n{i + 1}) [{videos[i].Duration}] *{videos[i].Title}*";
                }

                await channel.SendMessageAsync(str);
            }
        }

        public async Task AddAsync(string URL)
        {
            lock (locker)
            {
                if (!isReserved)
                    return;
            }

            await musicContainer?.AddAsync(URL);
        }

        private MusicContainer musicContainer = null;
        public async Task PlayAsync(string URL, IVoiceChannel voiceChannel, IMessageChannel channel)
        {
            try
            {
                await channel.SendMessageAsync("Підготовка сесії, відтворення незабаром розпочнеться…");

                musicContainer = new();

                await musicContainer.AddAsync(URL);

                _logger.LogInformation($"{DateTime.Now} Fetched {musicContainer.Count} videos");

                using (var audioClient = await voiceChannel.ConnectAsync())
                using (var voiceStream = audioClient.CreatePCMStream(AudioApplication.Music))
                {
                    var video = musicContainer.CurrentYoutubeVideo;

                    while (video is not null)
                    {
                        lock (locker)
                        {
                            if (!isReserved)
                                break;
                        }

                        await channel.SendMessageAsync($"Зараз відтворюється **{musicContainer.CurrIndex + 1}/{musicContainer.Count}**: [{video.Video.Duration}] ***{video.Video.Title}***");

                        var streamInfo = await video.StreamInfo;

                        _logger.LogInformation($"{DateTime.Now} Preparing video:{video.Video.Title}, container:{streamInfo.Container}");

                        await PlayVideoAsync(streamInfo, voiceStream);

                        video = musicContainer.NextYoutubeVideo;
                    }
                }
            }
            finally
            {
                await voiceChannel.DisconnectAsync();

                Stop();
            }
        }

        public void Pause() => isPlaying = false;

        public void Continue() => isPlaying = true;

        private bool isPlaying;
        private async Task PlayVideoAsync(IStreamInfo streamInfo, AudioOutStream stream)
        {
            var handle = Bass.CreateStream(streamInfo.Url, 0, BassFlags.Decode, null);

            if (handle == 0)
            {
                _logger.LogInformation($"{DateTime.Now} Handle init BASS Error: {Bass.LastError}");

                return;
            }

            _logger.LogInformation($"{DateTime.Now} Playing audiostream hID:{handle}");

            try
            {
                isPlaying = true;

                int count;

                do
                {
                    while (!isPlaying)
                        await Task.Delay(250);

                    lock (locker)
                    {
                        if (skip)
                        {
                            skip = false;
                            break;
                        }

                        if (!isReserved)
                            break;
                    }

                    byte[] buffer = new byte[256];

                    count = Bass.ChannelGetData(handle, buffer, buffer.Length);

                    if (count > 0)
                        await stream.WriteAsync(buffer, 0, count);
                } while (count >= 0);
            }
            finally
            {
                _logger.LogInformation($"{DateTime.Now} Handle exit BASS Error: {Bass.LastError}");

                Bass.StreamFree(handle);
            }
        }
    }
}
