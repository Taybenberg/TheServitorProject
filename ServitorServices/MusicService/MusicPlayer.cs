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

        /*
            if (url.Contains("youtube.com") || url.Contains("youtu.be"))
                return await YouTube.YouTubeAudio.GetAsync(url);
            else if (url.Contains("soundcloud.com"))
                return await SoundCloud.SoundCloudAudio.GetAsync(url, _soundcloudClientID);
            return new IAudio[0];
        */

        public void Dispose() => Bass.Free();

        public void Continue()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Shuffle()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}