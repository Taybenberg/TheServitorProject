using DataProcessor;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot : IHostedService, IDisposable
    {
        private readonly ILogger _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly DiscordSocketClient _client;

        private readonly string _seasonName;

        private readonly DateTime _seasonStart, _seasonEnd;

        private readonly ulong _destinyRoleId;

        private readonly ulong[] _channelId;
        private readonly ulong _raidChannelId;
        private readonly ulong _musicChannelId;
        private readonly ulong _lulzChannelId;
        private readonly ulong _bumpChannelId;

        public ServitorBot(IConfiguration configuration, ILogger<ServitorBot> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;

            _scopeFactory = scopeFactory;

            _client = new();

            _client.Log += LogAsync;

            _client.MessageReceived += OnMessageReceived;
            _client.ButtonExecuted += OnButtonExecuted;
            _client.SelectMenuExecuted += OnSelectMenuExecuted;
            //_client.MessageDeleted += OnMessageDeleted;

            _client.LoginAsync(TokenType.Bot, configuration["ApiKeys:DiscordToken"]).Wait();

            _seasonName = configuration["Destiny2:SeasonName"];
            _seasonStart = configuration.GetSection("Destiny2:SeasonStart").Get<DateTime>();
            _seasonEnd = configuration.GetSection("Destiny2:SeasonEnd").Get<DateTime>();

            _destinyRoleId = configuration.GetSection("DiscordConfig:DestinyRoleID").Get<ulong>();

            _channelId = configuration.GetSection("DiscordConfig:MainChannelID").Get<ulong[]>();
            _raidChannelId = configuration.GetSection("DiscordConfig:RaidChannelID").Get<ulong>();
            _musicChannelId = configuration.GetSection("DiscordConfig:MusicChannelID").Get<ulong>();
            _lulzChannelId = configuration.GetSection("DiscordConfig:LulzChannelID").Get<ulong>();
            _bumpChannelId = configuration.GetSection("DiscordConfig:BumpChannelID").Get<ulong>();

            _client.SetGameAsync("Destiny 2").Wait();

            RegisterExternalServices();
        }

        public void Dispose()
        {
            _raidManager.Dispose();

            _client.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken) =>
            await _client.StartAsync();

        public async Task StopAsync(CancellationToken cancellationToken) =>
            await _client.StopAsync();

        private Task OnSelectMenuExecuted(SocketMessageComponent arg)
        {
            Task.Run(async () => await OnSelectMenuExecutedAsync(arg));

            return Task.CompletedTask;
        }

        private Task OnButtonExecuted(SocketMessageComponent arg)
        {
            Task.Run(async () => await OnButtonExecutedAsync(arg));

            return Task.CompletedTask;
        }

        private Task OnMessageReceived(IMessage message)
        {
            Task.Run(async () => await OnMessageReceivedAsync(message));

            return Task.CompletedTask;
        }

        private Task OnMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            Task.Run(async () => await OnMessageDeletedAsync(arg1, arg2));

            return Task.CompletedTask;
        }

        private async Task LogAsync(LogMessage log) =>
            _logger.Log(log.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                _ => LogLevel.Information
            }, $"{DateTime.Now} {log.Exception?.ToString() ?? log.Message}");

        private IImageFactory getImageFactory()
        {
            using var scope = _scopeFactory.CreateScope();

            return scope.ServiceProvider.GetRequiredService<IImageFactory>();
        }

        private IDatabaseWrapperFactory getWrapperFactory()
        {
            using var scope = _scopeFactory.CreateScope();

            return scope.ServiceProvider.GetRequiredService<IDatabaseWrapperFactory>();
        }
    }
}
