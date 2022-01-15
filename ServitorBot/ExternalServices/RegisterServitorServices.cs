using ActivityService;
using BumperService;
using Microsoft.Extensions.DependencyInjection;
using MusicService;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private IBumpManager _bumper;
        private IActivityManager _activityManager;
        private IMusicPlayer _musicPlayer;

        private MusicPlayer _player;
        private RaidManager _raidManager;

        private void RegisterExternalServices()
        {
            using var scope = _scopeFactory.CreateScope();

            _bumper = scope.ServiceProvider.GetRequiredService<IBumpManager>();
            _bumper.OnNotify += OnBumperNotifyAsync;

            _activityManager = scope.ServiceProvider.GetRequiredService<IActivityManager>();
            /*
            _activityManager.ActivityNotification +=
            _activityManager.ActivityUpdated +=
            _activityManager.ActivityDisabled +=
            */
            _activityManager.Init();

            _musicPlayer = scope.ServiceProvider.GetRequiredService<IMusicPlayer>();
        }
    }
}
