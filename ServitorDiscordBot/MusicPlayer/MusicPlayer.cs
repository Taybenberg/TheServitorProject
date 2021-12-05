using Discord;
using Discord.Audio;
using ManagedBass;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using YoutubeExplode;
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

        private readonly object skipper = new();
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
            }
        }

        public void Next()
        {
            lock (skipper)
            {
                skip = true;
            }
        }

        public async Task Play(string URL, IVoiceChannel voiceChannel, IMessageChannel channel)
        {
            if (URL.Contains("list="))
            {
                await Playlist(URL, voiceChannel, channel);
                return;
            }

            try
            {
                await channel.SendMessageAsync("Буферизую відео, відтворення незабаром розпочнеться…");

                var youtube = new YoutubeClient();

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(URL);

                _logger.LogInformation($"{DateTime.Now} Fetched 1 video, url:{URL}");

                var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                using (var audioClient = await voiceChannel.ConnectAsync())
                using (var voiceStream = audioClient.CreatePCMStream(AudioApplication.Music))
                {
                    await PlayTrack(streamInfo, voiceStream);
                }
            }
            finally
            {
                await voiceChannel.DisconnectAsync();

                Stop();
            }
        }

        public async Task Playlist(string URL, IVoiceChannel voiceChannel, IMessageChannel channel)
        {
            try
            {
                await channel.SendMessageAsync("Буферизую плейлист, відтворення незабаром розпочнеться…");

                var youtube = new YoutubeClient();

                var videos = await youtube.Playlists.GetVideosAsync(URL);

                _logger.LogInformation($"{DateTime.Now} Fetched playlist size:{videos.Count}, url:{URL}");

                var streamInfos = videos.Select(async x =>
                {
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(x.Url);
                    return streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                });

                using (var audioClient = await voiceChannel.ConnectAsync())
                using (var voiceStream = audioClient.CreatePCMStream(AudioApplication.Music))
                {
                    foreach (var streamInfo in streamInfos)
                    {
                        lock (locker)
                        {
                            if (!isReserved)
                                break;
                        }

                        await PlayTrack(await streamInfo, voiceStream);
                    }
                }
            }
            finally
            {
                await voiceChannel.DisconnectAsync();

                Stop();
            }
        }

        private async Task PlayTrack(IStreamInfo streamInfo, AudioOutStream stream)
        {
            _logger.LogInformation($"{DateTime.Now} Preparing audiostream with container:{streamInfo.Container}, url:{streamInfo.Url}");
        
            var handle = Bass.CreateStream(streamInfo.Url, 0, BassFlags.Decode, null);

            if (handle == 0)
            {
                _logger.LogInformation($"{DateTime.Now} Handle init BASS Error: {Bass.LastError}");

                return;
            }

            _logger.LogInformation($"{DateTime.Now} Playing audiostream hID:{handle}");

            try
            {
                int count;

                do
                {
                    lock (skipper)
                    {
                        if (skip)
                        {
                            skip = false;
                            break;
                        }
                    }

                    lock (locker)
                    {
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
