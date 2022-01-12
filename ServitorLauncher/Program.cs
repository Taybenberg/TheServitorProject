using BumperDatabase;
using BumperService;
using BungieNetApi;
using ClanActivitiesDatabase;
using DataProcessor;
using Microsoft.EntityFrameworkCore;
using MusicService;
using RaidDatabase;
using RaidService;
using ServitorDiscordBot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IApiClient, ApiClient>(); //will be replaced

        services.AddDbContext<ClanActivitiesContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("ClanActivitiesDatabase")));
        services.AddScoped<IClanActivitiesDB, ClanActivitiesUoW>();
        services.AddScoped<IDatabaseWrapperFactory, DatabaseWrapperFactory>();

        services.AddDbContext<BumperContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("BumperDatabase")));
        services.AddScoped<IBumperDB, BumperUoW>();
        services.AddSingleton<IBumpManager, BumpManager>();

        services.AddDbContext<RaidContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("RaidDatabase")));
        services.AddScoped<IRaidDB, RaidUoW>();
        services.AddSingleton<IRaidManager, RaidManager>();

        services.AddSingleton<IMusicPlayer, MusicPlayer>();

        services.AddScoped<IParserFactory, ParserFactory>(); //will be removed
        services.AddScoped<IImageFactory, ImageFactory>(); //will be removed

        services.AddSingleton<ServitorBot>();
        services.AddHostedService(p => p.GetRequiredService<ServitorBot>());
    })
    .Build();

await host.RunAsync();