using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServitorDiscordBot
{
    public partial class ServitorBot : IHostedService
    {
        private DiscordSocketClient _client;

        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly string _discordToken;  
        private readonly ulong _destinyRoleID;
        private readonly ulong[] _mainChannelIDs;
        private readonly ulong[] _activityChannelIDs;
        private readonly ulong[] _musicChannelIDs;
        private readonly ulong[] _lulzChannelIDs;
        private readonly ulong[] _bumpChannelIDs;

        public ServitorBot(ILogger<ServitorBot> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            _discordToken = configuration["ApiKeys:DiscordToken"];

            _destinyRoleID = configuration.GetSection("DiscordConfig:DestinyRoleID").Get<ulong>();

            _mainChannelIDs = configuration.GetSection("DiscordConfig:MainChannelID").Get<ulong[]>();
            _activityChannelIDs = configuration.GetSection("DiscordConfig:ActivityChannelID").Get<ulong[]>();
            _musicChannelIDs = configuration.GetSection("DiscordConfig:MusicChannelID").Get<ulong[]>();
            _lulzChannelIDs = configuration.GetSection("DiscordConfig:LulzChannelID").Get<ulong[]>();
            _bumpChannelIDs = configuration.GetSection("DiscordConfig:BumpChannelID").Get<ulong[]>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new();

            _client.Log += OnLogAsync;

            _client.MessageReceived += (arg) => 
            { 
                Task.Run(async () => await OnMessageReceivedAsync(arg));
                return Task.CompletedTask;
            };

            _client.MessageDeleted += (arg1, arg2) =>
            {
                Task.Run(async () => await OnMessageDeletedAsync(arg1.Id, arg2.Id));
                return Task.CompletedTask;
            };

            _client.ButtonExecuted += (arg) =>
            {
                Task.Run(async () => await OnButtonExecutedAsync(arg)); 
                return Task.CompletedTask;
            };

            _client.SelectMenuExecuted += (arg) =>
            {
                Task.Run(async () => await OnSelectMenuExecutedAsync(arg));
                return Task.CompletedTask;
            };

            _client.SlashCommandExecuted += (arg) =>
            {
                Task.Run(async () => await OnSlashCommandExecutedAsync(arg));
                return Task.CompletedTask;
            };

            await _client.LoginAsync(TokenType.Bot, _discordToken);
            await _client.StartAsync();
            await _client.SetGameAsync("Destiny 2");

            await RegisterSlashCommandsAsync();
            await RegisterExternalServicesAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.StopAsync();
            await _client.DisposeAsync();
        }

        private async Task OnLogAsync(LogMessage log) =>
            _logger.Log(log.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                _ => LogLevel.Information
            }, $"{DateTime.Now} {log.Exception?.ToString() ?? log.Message}");
    }
}
