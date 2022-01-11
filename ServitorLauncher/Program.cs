using BungieNetApi;
using BumperDatabase;
using BumperService;
using Database;
using DataProcessor;
using Microsoft.EntityFrameworkCore;
using ServitorDiscordBot;
using RaidDatabase;
using RaidService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IApiClient, ApiClient>(); //will be replaced

        services.AddDbContext<ClanContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("ClanActivitiesDatabase")));
        services.AddScoped<IClanDB, ClanUoW>();

        services.AddDbContext<BumperContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("BumperDatabase")));
        services.AddScoped<IBumperDB, BumperUoW>();
        services.AddSingleton<IBumpManager, BumpManager>();

        services.AddDbContext<RaidContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("RaidDatabase")));
        services.AddScoped<IRaidDB, RaidUoW>();
        services.AddSingleton<IRaidManager, RaidManager>();

        services.AddScoped<IParserFactory, ParserFactory>(); //will be removed
        services.AddScoped<IImageFactory, ImageFactory>(); //will be removed
        services.AddScoped<IDatabaseWrapperFactory, DatabaseWrapperFactory>(); //will be removed

        services.AddSingleton<ServitorBot>();
        services.AddHostedService(p => p.GetRequiredService<ServitorBot>());
    })
    .Build();

await host.RunAsync();