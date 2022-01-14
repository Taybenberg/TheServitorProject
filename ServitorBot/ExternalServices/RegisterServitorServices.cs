using ActivityService;
using BumperService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicService;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private IBumpManager _bumper;
        private IActivityManager _raider;
        //private IMusicPlayer _player;

        private MusicPlayer _player;
        private RaidManager _raidManager;

        private void RegisterExternalServices(IConfiguration configuration, ILogger<ServitorBot> logger)
        {
            using var scope = _scopeFactory.CreateScope();

            _bumper = scope.ServiceProvider.GetRequiredService<IBumpManager>();
            _bumper.Notify += BumperNotifyAsync;

            _raider = scope.ServiceProvider.GetRequiredService<IActivityManager>();
            /*
            _raider.ActivityNotification += _;
            _raider.ActivityUpdated += _;
            _raider.ActivityDeleted += _;
            _raider.Init();
            */

            //_player = scope.ServiceProvider.GetRequiredService<IMusicPlayer>();
        }
    }
}
