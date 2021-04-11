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

        private readonly string _serverIconUrl, _clanUrl, _serverName, _seasonName;

        private readonly DateTime _seasonStart;

        private readonly ulong _channelId, _bumpChannelId;

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

            _serverIconUrl = configuration["Discord:ServerIconURL"];
            _serverName = configuration["Discord:ServerName"];
            _channelId = configuration.GetSection("Discord:MainChannelID").Get<ulong>();
            _bumpChannelId = configuration.GetSection("Discord:BumpChannelID").Get<ulong>();
            _bumpPingUsers = configuration.GetSection("Discord:BumpPingUsers").Get<string[]>();

            _client.SetGameAsync("Destiny 2").Wait();

            _bumper = new();
            _bumper.Notify += Bumper_Notify;

            _logger.LogInformation($"{DateTime.Now} Bump scheduled on {_bumper.NextBump}");
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _client.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.StopAsync();
        }

        private Task LogAsync(LogMessage log)
        {
            var logLevel = log.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Error => LogLevel.Error,
                _ => LogLevel.Information
            };

            _logger.Log(logLevel, $"{DateTime.Now} {log.Exception?.ToString() ?? log.Message}");

            return Task.CompletedTask;
        }

        private EmbedFooterBuilder GetFooter()
        {
            var footer = new EmbedFooterBuilder();

            footer.IconUrl = _client.CurrentUser.GetAvatarUrl();
            footer.Text = $"Ваш відданий {_client.CurrentUser.Username}";

            return footer;
        }
    }
}
