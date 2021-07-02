using BungieNetApi;
using Database;
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

        private readonly Bumper _bumper;

        private readonly string _clanUrl, _seasonName;

        private readonly DateTime _seasonStart;

        private readonly ulong[] _channelId;
        private readonly ulong _bumpChannelId;

        private readonly string[] _bumpPingUsers;

        public ServitorBot(IConfiguration configuration, ILogger<ServitorBot> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;

            _scopeFactory = scopeFactory;

            _client = new();

            _client.Log += LogAsync;

            _client.MessageReceived += MessageReceivedAsync;

            _client.LoginAsync(TokenType.Bot, configuration["Discord:BotToken"]).Wait();

            _clanUrl = configuration["Destiny2:ClanURL"];
            _seasonName = configuration["Destiny2:SeasonName"];
            _seasonStart = configuration.GetSection("Destiny2:SeasonStart").Get<DateTime>();

            _channelId = configuration.GetSection("Discord:MainChannelID").Get<ulong[]>();
            _bumpChannelId = configuration.GetSection("Discord:BumpChannelID").Get<ulong>();
            _bumpPingUsers = configuration.GetSection("Discord:BumpPingUsers").Get<string[]>();

            _client.SetGameAsync("Destiny 2").Wait();

            _bumper = new();
            _bumper.Notify += Bumper_Notify;

            _logger.LogInformation($"{DateTime.Now} Bump scheduled on {_bumper.NextBump}");
        }

        public void Dispose() =>
            _client.Dispose();

        public async Task StartAsync(CancellationToken cancellationToken) =>
            await _client.StartAsync();

        public async Task StopAsync(CancellationToken cancellationToken) =>
            await _client.StopAsync();

        private async Task LogAsync(LogMessage log) =>
            _logger.Log(log.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                _ => LogLevel.Information
            }, $"{DateTime.Now} {log.Exception?.ToString() ?? log.Message}");

        private IClanDB getDatabase() =>
            _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IClanDB>();

        private IParserFactory getFactory() =>
            _scopeFactory.CreateScope().ServiceProvider
            .GetRequiredService<IParserFactory>();

        private IApiClient getApiClient() =>
            _scopeFactory.CreateScope().ServiceProvider
            .GetRequiredService<IApiClient>();
    }
}
