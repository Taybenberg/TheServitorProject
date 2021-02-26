﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Coravel;
using ServitorDiscordBot;
using BungieNetApi;
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
                    var db = host.Services.GetService<ClanDatabase>();

                    await db.SyncUsersAsync();

                    await db.SyncActivitiesAsync();
                }).Cron("0 */6 * * *");
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((host, services) =>
                {
                    services.AddScheduler();

                    services.AddScoped<BungieNetApiClient>();

                    services.AddDbContext<ClanDatabase>(options => options.UseSqlite(host.Configuration.GetConnectionString("ClanDatabase")));
                    
                    services.AddHostedService<ServitorBot>();
                });
    }
}
