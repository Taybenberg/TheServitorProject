using BungieNetApi;
using Database;
using DataProcessor;
using Microsoft.EntityFrameworkCore;
using ServitorDiscordBot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IApiClient, ApiClient>();

        services.AddDbContext<ClanContext>(options => options.UseSqlite(hostContext.Configuration.GetConnectionString("ClanDatabase")));

        services.AddScoped<IClanDB, ClanUoW>();

        services.AddScoped<IParserFactory, ParserFactory>();
        services.AddScoped<IImageFactory, ImageFactory>();
        services.AddScoped<IDatabaseWrapperFactory, DatabaseWrapperFactory>();

        services.AddSingleton<ServitorBot>();
        services.AddHostedService(p => p.GetRequiredService<ServitorBot>());
    })
    .Build();

await host.RunAsync();