using Discord;
using Discord.Audio;
using ManagedBass;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
        private bool isLimitedMode = false;
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
                isLimitedMode = false;
                isPlaying = true;
            }
        }

        public void Next()
        {
            lock (locker)
            {
                if (isLimitedMode)
                    return;

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
                if (isLimitedMode)
                    return;

                if (isReserved)
                    skip = true;

                isPlaying = true;
            }
        }

        public void Shuffle()
        {
            lock (locker)
            {
                if (!isReserved || isLimitedMode)
                    return;
            }

            musicContainer?.Shuffle();
        }

        public async Task GetQueueAsync(IMessageChannel channel)
        {
            lock (locker)
            {
                if (!isReserved || isLimitedMode)
                    return;
            }

            var curr = musicContainer?.CurrIndex;
            var videos = musicContainer?.AllAudios;

            if (curr is not null && videos is not null)
            {
                int count = videos.Length;

                string str = $"У черзі {count} відео:";

                for (int i = 0; i < count; i++)
                {
                    string tmp;

                    if (i == curr)
                        tmp = $"\n**{i + 1})** [{videos[i].Duration}] ***{videos[i].Title}***";
                    else
                        tmp = $"\n{i + 1}) [{videos[i].Duration}] *{videos[i].Title}*";

                    if ((str + tmp).Length < 2000)
                        str += tmp;
                    else break;
                }

                await channel.SendMessageAsync(str);
            }
        }

        public async Task AddAsync(string URL)
        {
            lock (locker)
            {
                if (!isReserved || isLimitedMode)
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
                    var audio = musicContainer.CurrentAudio;

                    while (audio is not null)
                    {
                        lock (locker)
                        {
                            if (!isReserved)
                                break;
                        }

                        await channel.SendMessageAsync($"Зараз відтворюється **{musicContainer.CurrIndex + 1}/{musicContainer.Count}**: [{audio.Duration}] ***{audio.Title}***");

                        _logger.LogInformation($"{DateTime.Now} Preparing audio:{audio.Title}");

                        await PlayStreamAsync(await audio.URL, voiceStream);

                        audio = musicContainer.NextAudio;
                    }
                }
            }
            finally
            {
                await voiceChannel.DisconnectAsync();

                Stop();
            }
        }

        public async Task PlayDirectAsync(string URL, IVoiceChannel voiceChannel, IMessageChannel channel)
        {
            try
            {
                lock (locker)
                {
                    isLimitedMode = true;
                }

                musicContainer = null;

                await channel.SendMessageAsync("Підготовка сесії, відтворення незабаром розпочнеться…");

                _logger.LogInformation($"{DateTime.Now} Preparing direct stream {URL}");

                using (var audioClient = await voiceChannel.ConnectAsync())
                using (var voiceStream = audioClient.CreatePCMStream(AudioApplication.Music))
                {
                    await PlayStreamAsync(URL, voiceStream);
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
        private async Task PlayStreamAsync(string URL, AudioOutStream stream)
        {
            var handle = Bass.CreateStream(URL, 0, BassFlags.Decode, null);

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
