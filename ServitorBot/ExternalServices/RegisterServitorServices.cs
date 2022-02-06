using ActivityService;
using BumperService;
using Microsoft.Extensions.DependencyInjection;
using MusicService;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private IBumpManager _bumper;
        private IActivityManager _activityManager;
        private IMusicPlayer _musicPlayer;

        private async Task RegisterExternalServicesAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            _bumper = scope.ServiceProvider.GetRequiredService<IBumpManager>();
            _bumper.OnNotify += OnBumperNotifyAsync;

            _activityManager = scope.ServiceProvider.GetRequiredService<IActivityManager>();
            _activityManager.OnNotification += OnActivityNotificationAsync;
            _activityManager.OnUpdated += OnActivityUpdatedAsync;
            _activityManager.OnDisabled += OnActivityDisabledAsync;
            _activityManager.OnCreated += OnActivityCreatedAsync;
            _activityManager.OnRescheduled += OnActivityRescheduledAsync;
            await _activityManager.InitAsync();

            _musicPlayer = scope.ServiceProvider.GetRequiredService<IMusicPlayer>();
            _musicPlayer.OnUpdate += OnMusicPlayerUpdateAsync;
        }
    }
}
