using BungieNetApi;
using Coravel;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServitorDiscordBot;
using System;

namespace BotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                scheduler.ScheduleAsync(async () =>
                {
                    var db = host.Services.GetService<ClanDatabase>();

                    await db.SyncUsersAsync();

                    await db.SyncActivitiesAsync();
                }).DailyAtHour(5).Zoned(TimeZoneInfo.Local);

                scheduler.ScheduleAsync(async () =>
                {
                    var bot = host.Services.GetService<ServitorBot>();

                    await bot.XurNotificationAsync();
                }).DailyAtHour(17).Friday();
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

                    services.AddSingleton<ServitorBot>();
                    services.AddHostedService(p => p.GetRequiredService<ServitorBot>());
                });
    }
}
