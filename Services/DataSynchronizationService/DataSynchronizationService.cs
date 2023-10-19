using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Utils;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Utils.Youtube;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Services.UserService;

namespace MediaTrackerYoutubeService.Services.DataSynchronizationService;

public class DataSynchronizationService : IDataSynchronizationService
{
    private readonly IDbContextFactory<AppDbContext> _context;
    private readonly IAuthTokenExchangeService _authTokenExchangeService;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    
    public DataSynchronizationService(IDbContextFactory<AppDbContext> context, IAuthTokenExchangeService authTokenExchangeService, IUserService userService, IConfiguration configuration)
    {
        _context = context;
        _authTokenExchangeService = authTokenExchangeService;
        _userService = userService;
        _configuration = configuration;
    }

    public async void SyncData(int userId){
        try 
        {
            if (!(await _userService.UpsertUser(userId)).Success) throw new Exception("Failed to add new user or confirm exisiting user");
            
            var tokenResult = await _authTokenExchangeService.YoutubeAuthTokenExchange(userId);
            
            if (!tokenResult.Success) throw new Exception("Failed to get youtube token");
            
            var access_token = tokenResult.Data;
            var apiKey = _configuration["YoutubeAPIKey"] ?? throw new Exception("Missing ApiKey in Config");
            
            YoutubeAPIClient client = new YoutubeAPIClient(access_token, apiKey);
            
            
            await Task.WhenAll(SyncPlaylistsAndAssociatedVideos(userId, client),
                                SyncSubscriptionsAndAssociatedChannels(userId, client));
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
        }
    }

    private async Task SyncPlaylistsAndAssociatedVideos(int userId, YoutubeAPIClient client){
        try {
            var getUserResponse = await _userService.GetUser(userId);
            if (!getUserResponse.Success) throw new Exception("Failed to get User");
            var playlistsResponse = await client.GetMyPlaylists();


            // playlistsInternal.
            


            var playlistsExternal = playlistsResponse.items;
            var playlistsInternal = getUserResponse.Data!.VideoPlaylists;

            // foreach (var playlist in playlists) {
                
            // }


        } catch {

        }
    }

    private async Task SyncSubscriptionsAndAssociatedChannels(int userId, YoutubeAPIClient client){
        throw new NotImplementedException();
    }
}