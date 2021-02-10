﻿using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Coravel;
using ServitorDiscordBot;
using Database;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            
            host.Services.UseScheduler(scheduler => {
                scheduler.ScheduleAsync(async () =>
                {
                    await host.Services.GetService<ClanDatabase>().SyncUserRelationsAsync();
                }).DailyAt(5, 40).Zoned(TimeZoneInfo.Local);

                scheduler.ScheduleAsync(async () =>
                {
                    await host.Services.GetService<ClanDatabase>().SyncUsersAsync();
                }).DailyAt(5, 0).Zoned(TimeZoneInfo.Local);

                scheduler.ScheduleAsync(async () =>
                {
                    await host.Services.GetService<ClanDatabase>().SyncActivitiesAsync();
                }).DailyAt(5, 10).Zoned(TimeZoneInfo.Local);
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddScheduler();
                    services.AddSingleton<ClanDatabase>();
                    services.AddHostedService<ServitorBot>();
                });
    }
}