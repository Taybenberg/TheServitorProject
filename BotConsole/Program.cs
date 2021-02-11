using System;
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
                    await host.Services.GetService<ClanDatabase>().SyncUsersAsync();

                    await host.Services.GetService<ClanDatabase>().SyncActivitiesAsync();

                    await host.Services.GetService<ClanDatabase>().SyncUserRelationsAsync();
                }).DailyAt(5, 0).Zoned(TimeZoneInfo.Local);
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((host, services) =>
                {
                    services.AddScheduler();

                    services.AddSingleton<BungieNetApiClient>();

                    services.AddDbContext<ClanDatabase>(options => options.UseSqlite(host.Configuration.GetConnectionString("ClanDatabase")));
                    
                    services.AddHostedService<ServitorBot>();
                });
    }
}
