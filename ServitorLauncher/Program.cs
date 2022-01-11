using BungieNetApi;
using BumperDatabase;
using BumperService;
using Database;
using DataProcessor;
using Microsoft.EntityFrameworkCore;
using ServitorDiscordBot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IApiClient, ApiClient>();

        services.AddDbContext<ClanContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("ClanActivitiesDatabase")));
        services.AddScoped<IClanDB, ClanUoW>();

        services.AddDbContext<BumperContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("BumperDatabase")));
        services.AddScoped<IBumperDB, BumperUoW>();
        services.AddSingleton<IBumper, Bumper>();

        //services.AddDbContext<ClanContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("RaidDatabase")));

        services.AddScoped<IParserFactory, ParserFactory>();
        services.AddScoped<IImageFactory, ImageFactory>();
        services.AddScoped<IDatabaseWrapperFactory, DatabaseWrapperFactory>();
        
        services.AddSingleton<ServitorBot>();
        services.AddHostedService(p => p.GetRequiredService<ServitorBot>());
    })
    .Build();

await host.RunAsync();