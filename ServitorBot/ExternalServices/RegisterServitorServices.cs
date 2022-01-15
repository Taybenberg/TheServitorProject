﻿using ActivityService;
using BumperService;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private IBumpManager _bumper;
        private IActivityManager _raider;
        //private IMusicPlayer _player;

        private MusicPlayer _player;
        private RaidManager _raidManager;

        private void RegisterExternalServices()
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
