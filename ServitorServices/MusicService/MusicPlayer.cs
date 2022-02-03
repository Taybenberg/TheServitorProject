using ManagedBass;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace MusicService
{
    public class MusicPlayer : IDisposable, IMusicPlayer
    {
        private readonly ILogger _logger;
        private readonly string _soundcloudClientID;

        public MusicPlayer(ILogger<MusicPlayer> logger, IConfiguration configuration)
        {
            (_logger, _soundcloudClientID) = (logger, configuration["ApiKeys:SoundCloudClientID"]);

            Bass.Init(0);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Bass.PluginLoad("basshls.dll");
                Bass.PluginLoad("bassopus.dll");
                Bass.PluginLoad("basswebm.dll");
            }
            else
            {
                Bass.PluginLoad("libbasshls.so");
                Bass.PluginLoad("libbassopus.so");
                Bass.PluginLoad("libbasswebm.so");
            }
        }

        public void Dispose() => Bass.Free();

        public event Func<ulong?, Task> OnUpdate;

        private readonly object _locker = new();

        private bool _isReserved = false;
        private bool _isLimitedMode = false;
        private bool _skip = false;
        private bool _isPlaying = false;

        private ulong? _instanceID = null;

        private MusicContainer _musicContainer = null;

        public bool IsPlaying => _isPlaying;

        public IEnumerable<(bool, IAudio)> Queue
        {
            get
            {
                lock (_locker)
                {
                    if (!_isReserved || _isLimitedMode)
                        return null;
                }

                var current = _musicContainer?.CurrentIndex;
                var audios = _musicContainer?.AllAudios;

                if (current is not null && audios is not null)
                {
                    return audios.Select((x, i) => (i == current, x));
                }

                return null;
            }
        }

        public bool Init()
        {
            lock (_locker)
            {
                if (!_isReserved)
                {
                    _isReserved = true;
                    _skip = false;

                    return true;
                }

                return false;
            }
        }

        public void Next()
        {
            lock (_locker)
            {
                if (_isLimitedMode)
                    return;

                if (_isReserved)
                    _skip = true;

                _isPlaying = true;
            }
        }

        public void Stop()
        {
            lock (_locker)
            {
                _isReserved = false;
                _isLimitedMode = false;
                _isPlaying = true;
            }
        }

        public void Previous()
        {
            _musicContainer?.GetPreviousNext();

            lock (_locker)
            {
                if (_isLimitedMode)
                    return;

                if (_isReserved)
                    _skip = true;

                _isPlaying = true;
            }
        }

        public void Shuffle()
        {
            lock (_locker)
            {
                if (!_isReserved || _isLimitedMode)
                    return;
            }

            _musicContainer?.Shuffle();

            OnUpdate?.Invoke(_instanceID);
        }

        public void Continue()
        {
            _isPlaying = true;

            OnUpdate?.Invoke(_instanceID);
        }

        public void Pause()
        {
            _isPlaying = false;

            OnUpdate?.Invoke(_instanceID);
        }

        public async Task AddAsync(string URL)
        {
            lock (_locker)
            {
                if (!_isReserved || _isLimitedMode)
                    return;
            }

            await _musicContainer?.AddAudioAsync(URL);

            OnUpdate?.Invoke(_instanceID);
        }

        public async Task PlayAsync(string url, Stream audioOutStream, ulong instanceID)
        {
            _musicContainer = new(_soundcloudClientID);

            _instanceID = instanceID;

            await _musicContainer.AddAudioAsync(url);

            var audio = _musicContainer.CurrentAudio;

            while (audio is not null)
            {
                lock (_locker)
                {
                    if (!_isReserved)
                        break;
                }

                _isPlaying = true;

                OnUpdate?.Invoke(_instanceID);

                await PlayStreamAsync(await audio.URL, audioOutStream);

                audio = _musicContainer.NextQueuedAudio;
            }

            _musicContainer = null;
            _instanceID = null;
        }

        public async Task PlayDirectAsync(string URL, Stream audioOutStream)
        {
            lock (_locker)
            {
                _isLimitedMode = true;
            }

            _isPlaying = true;

            await PlayStreamAsync(URL, audioOutStream);
        }

        private async Task PlayStreamAsync(string URL, Stream stream)
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
                int count;

                do
                {
                    while (!_isPlaying)
                        await Task.Delay(250);

                    lock (_locker)
                    {
                        if (_skip)
                        {
                            _skip = false;
                            break;
                        }

                        if (!_isReserved)
                            break;
                    }

                    byte[] buffer = new byte[256];

                    count = Bass.ChannelGetData(handle, buffer, buffer.Length);

                    if (count > 0)
                        await stream.WriteAsync(buffer, 0, count);
                } while (count >= 0);

                await stream.FlushAsync();
            }
            finally
            {
                _logger.LogInformation($"{DateTime.Now} Handle exit BASS Error: {Bass.LastError}");

                Bass.StreamFree(handle);
            }
        }
    }
}