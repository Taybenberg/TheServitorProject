﻿using ActivityService;
using BumperService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private IBumpManager _bumper;
        private IActivityManager _raider;

        private RaidManager _raidManager;

        private MusicPlayer _player;

        private void RegisterExternalServices(IConfiguration configuration, ILogger<ServitorBot> logger)
        {
            _raidManager = new(logger);
            _raidManager.Notify += Event_Notify;
            _raidManager.Update += Event_Update;
            _raidManager.Delete += Event_Delete;
            _raidManager.Load();

            _player = new(logger, configuration["ApiKeys:SoundCloudClientID"]);

            using var scope = _scopeFactory.CreateScope();

            _bumper = scope.ServiceProvider.GetRequiredService<IBumpManager>();
            _bumper.Notify += BumperNotifyAsync;

            _raider = scope.ServiceProvider.GetRequiredService<IActivityManager>();
            //raider
        }
    }
}
