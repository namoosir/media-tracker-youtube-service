using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Utils;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Utils.Youtube;

namespace MediaTrackerYoutubeService.Services.DataSynchronizationService;

public class DataSynchronizationService : IDataSynchronizationService
{
    private readonly IDbContextFactory<AppDbContext> _context;
    public DataSynchronizationService(IDbContextFactory<AppDbContext> context)
    {
        _context = context;
    }

    public void SyncData(int userId){
        // string access_token = FetchAccessTokenFromAuth(userId);
        // YoutubeAPIClient client = new YoutubeAPIClient(access_token, "SDFKJDSHNJK");
        // SyncPlaylistsAndAssociatedVideos();
        // SyncSubscriptionsAndAssociatedChannels();
    }

    private void SyncPlaylistsAndAssociatedVideos(YoutubeAPIClient client){

    }

    private void SyncSubscriptionsAndAssociatedChannels(YoutubeAPIClient client){
        
    }
}