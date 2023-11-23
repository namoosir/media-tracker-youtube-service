using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.GraphQL;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using MediaTrackerYoutubeService.Services.FetchYoutubeDataService;
using MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;
using MediaTrackerYoutubeService.Services.StoreYoutubeDataService;
using MediaTrackerYoutubeService.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using MediaTrackerYoutubeService.Services.DataSynchronizationService;
using MediaTrackerYoutubeService.Services.PlaylistService;
using MediaTrackerYoutubeService.Services.UserService;
using MediaTrackerYoutubeService.Services.VideoService;
using MediaTrackerYoutubeService.Services.ChannelService;

public class TestStartup
{
    public static IServiceProvider ConfigureServices()
    {
        // var configuration = new ConfigurationBuilder()
        // .AddInMemoryCollection(new Dictionary<string, string>
        // {
        //     {"ConnectionStrings:DBConnectionString", "Data Source=YourServer;Initial Catalog=YourDatabase;User ID=YourUser;Password=YourPassword;"},
        //     {"AppSettings:SomeSetting", "SomeValue"},
        //     // Add more key-value pairs as needed
        // })
        // .Build();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);

        // Add services to the container.
        services.AddPooledDbContextFactory<AppDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DBConnectionString"))
        );

        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DBConnectionString"))
        );

        services
            .AddGraphQLServer()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
            .AddQueryType<Query>()
            .AddProjections()
            .AddFiltering()
            .AddSorting();

        services.AddHttpClient();

        services.AddScoped<IAuthTokenExchangeService, AuthTokenExchangeService>();
        services.AddScoped<IFetchYoutubeDataService, FetchYoutubeDataService>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IProcessYoutubeDataService, ProcessYoutubeDataService>();
        services.AddScoped<IStoreYoutubeDataService, StoreYoutubeDataService>();
        services.AddScoped<IDataSynchronizationService, DataSynchronizationService>();
        services.AddScoped<IPlaylistService, PlaylistService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVideoService, VideoService>();

        services.AddScoped<QueryInspectionMiddleware>();

        services.AddAutoMapper(typeof(Program).Assembly);

        return services.BuildServiceProvider();
    }
}
