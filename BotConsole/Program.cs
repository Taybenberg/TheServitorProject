using BungieNetApi;
using Coravel;
using Database;
using DataProcessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServitorDiscordBot;
using System;
using System.Threading.Tasks;

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
                    await syncDB(host);
                }).DailyAtHour(5).Zoned(TimeZoneInfo.Local);

                scheduler.ScheduleAsync(async () =>
                {
                    var bot = host.Services.GetService<ServitorBot>();

                    await bot.DailyResetNotificationAsync();

                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Tuesday:
                            await bot.WeeklyResetNotificationAsync();
                            break;

                        case DayOfWeek.Friday:
                            await bot.XurNotificationAsync();
                            break;
                    }
                }).DailyAtHour(17);
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((host, services) =>
                {
                    services.AddScheduler();

                    services.AddScoped<IApiClient, ApiClient>();

                    services.AddDbContext<ClanContext>(options => options.UseSqlite(host.Configuration.GetConnectionString("ClanDatabase")));

                    services.AddScoped<IClanDB, ClanUoW>();

                    services.AddScoped<IParserFactory, ParserFactory>();
                    services.AddScoped<IImageFactory, ImageFactory>();
                    services.AddScoped<IDatabaseWrapperFactory, DatabaseWrapperFactory>();

                    services.AddSingleton<ServitorBot>();
                    services.AddHostedService(p => p.GetRequiredService<ServitorBot>());
                });

        private static async Task syncDB(IHost host)
        {
            var db = host.Services.GetService<IClanDB>();

            await db.SyncUsersAsync();

            await db.SyncActivitiesAsync();
        }
    }
}
