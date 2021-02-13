﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot : IHostedService, IDisposable
    {
        private readonly ILogger _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly DiscordSocketClient _client;

        public ServitorBot(IConfiguration configuration, ILogger<ServitorBot> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;

            _scopeFactory = scopeFactory;

            _client = new DiscordSocketClient();

            _client.Log += LogAsync;

            _client.MessageReceived += MessageReceivedAsync;

            _client.LoginAsync(TokenType.Bot, configuration["DiscordBotToken"]).Wait();

            _client.SetGameAsync("Destiny 2").Wait();
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
                LogSeverity.Debug =>  LogLevel.Debug,
                LogSeverity.Error =>  LogLevel.Error,
                _ => LogLevel.Information
            };

            _logger.Log(logLevel, $"{DateTime.Now} {log.Exception?.ToString() ?? log.Message}");

            return Task.CompletedTask;
        }
    }
}
