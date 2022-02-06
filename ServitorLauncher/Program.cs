using ActivityDatabase;
using ActivityService;
using BumperDatabase;
using BumperService;
using BungieSharper.Client;
using ClanActivitiesDatabase;
using ClanActivitiesService;
using Coravel;
using DestinyNotificationsDatabase;
using DestinyNotificationsService;
using Microsoft.EntityFrameworkCore;
using MusicService;
using ServitorBot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScheduler();

        services.AddScoped(p => new BungieApiClient(new BungieClientConfig
        {
            ApiKey = hostContext.Configuration["ApiKeys:BungieApiKey"]
        }));

        services.AddDbContext<ClanActivitiesContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("ClanActivitiesDatabase")));
        services.AddScoped<IClanActivitiesDB, ClanActivitiesUoW>();
        services.AddScoped<IClanActivities, ClanActivitiesManager>();

        services.AddDbContext<NotificationsContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("NotificationsDatabase")));
        services.AddScoped<INotificationsDB, NotificationsUoW>();
        services.AddScoped<IDestinyNotifications, DestinyNotificationsManager>();

        services.AddDbContext<BumperContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("BumperDatabase")));
        services.AddScoped<IBumperDB, BumperUoW>();
        services.AddSingleton<IBumpManager, BumpManager>();

        services.AddDbContext<ActivityContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("ActivityDatabase")));
        services.AddScoped<IActivityDB, ActivityUoW>();
        services.AddSingleton<IActivityManager, ActivityManager>();

        services.AddSingleton<IMusicPlayer, MusicPlayer>();

        services.AddSingleton<ServitorDiscordBot>();
        services.AddHostedService(p => p.GetRequiredService<ServitorDiscordBot>());
    })
    .Build();

host.Services.UseScheduler(scheduler =>
{
    scheduler.ScheduleAsync(async () =>
    {
        using var scope = host.Services.CreateScope();

        var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

        await clanActivities.SyncDatabaseAsync();
    }).DailyAtHour(5);
});

await host.RunAsync();