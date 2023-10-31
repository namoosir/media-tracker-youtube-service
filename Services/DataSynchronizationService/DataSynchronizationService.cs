using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Utils;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Utils.Youtube;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Services.UserService;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.DataSynchronizationService;

public class DataSynchronizationService : IDataSynchronizationService
{
    private readonly IDbContextFactory<AppDbContext> _context;
    private readonly IAuthTokenExchangeService _authTokenExchangeService;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    private const double REFRESH_DELAY = 10.0;

    public DataSynchronizationService(
        IDbContextFactory<AppDbContext> context,
        IAuthTokenExchangeService authTokenExchangeService,
        IUserService userService,
        IConfiguration configuration
    )
    {
        _context = context;
        _authTokenExchangeService = authTokenExchangeService;
        _userService = userService;
        _configuration = configuration;
    }

    public async void SyncData(int userId)
    {
        try
        {
            if (!(await _userService.UpsertUser(userId)).Success)
                throw new Exception("Failed to add new user or confirm exisiting user");

            var tokenResult = await _authTokenExchangeService.YoutubeAuthTokenExchange(userId);

            if (!tokenResult.Success)
                throw new Exception("Failed to get youtube token");

            var access_token = tokenResult.Data;
            var apiKey =
                _configuration["YoutubeAPIKey"] ?? throw new Exception("Missing ApiKey in Config");

            YoutubeAPIClient client = new YoutubeAPIClient(access_token, apiKey);

            await Task.WhenAll(
                SyncPlaylistsAndAssociatedVideos(userId, client),
                SyncSubscriptionsAndAssociatedChannels(userId, client)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task SyncPlaylistsAndAssociatedVideos(int userId, YoutubeAPIClient client)
    {
        try
        {
            //
            // playlistsExternal = fetchService....
            // (toUpdate, toInsert, toDelete) = processService(playlistExternal)
            // storeService.store((toUpdate, toInsert, toDelete))
            //

            var getUserResponse = await _userService.GetUser(userId);

            if (!getUserResponse.Success)
                throw new Exception("Failed to get User");

            var user = getUserResponse!.Data;
            var playlistsInternal = user!.VideoPlaylists;

            List<Resource> playlistsExternal = new();
            var playlistsResponse = await client.GetMyPlaylists();

            playlistsExternal.AddRange(playlistsResponse.items);

            while (playlistsResponse.nextPageToken != null)
            {
                playlistsResponse = await client.GetMyPlaylists(playlistsResponse.nextPageToken);
                playlistsExternal.AddRange(playlistsResponse.items);
            }

            List<Playlist> playlistsToUpdate = new();
            List<Playlist> playlistsToInsert = new();
            List<Playlist> playlistsToDelete = new();

            foreach (var pe in playlistsExternal)
            {
                var foundInternal = playlistsInternal.FirstOrDefault(x => x.YoutubeId == pe.id);

                //create new playlist object and append to playlistsToInsert
                if (foundInternal == null)
                {
                    var playlistRecord = new Playlist
                    {
                        User = user,
                        YoutubeId = pe.id,
                        ETag = pe.etag,
                        Videos = new List<Video>()
                    };

                    playlistsToInsert.Add(playlistRecord);
                    playlistsToUpdate.Add(playlistRecord);
                }
                //add updated element to playlistsToUpdate
                else if (
                    (DateTime.Now - foundInternal.UpdatedAt).TotalMinutes >= REFRESH_DELAY
                    && foundInternal.ETag != pe.etag
                )
                {
                    foundInternal.ETag = pe.etag;
                    playlistsToUpdate.Add(foundInternal);
                }
            }

            //create list of playlists to delete
            foreach (var pi in playlistsInternal)
            {
                var foundExternal = playlistsExternal.FirstOrDefault(x => x.id == pi.YoutubeId);

                if (foundExternal == null)
                {
                    playlistsToDelete.Add(pi);
                }
            }
        }
        catch (Exception e) { }
    }

    private async Task SyncSubscriptionsAndAssociatedChannels(int userId, YoutubeAPIClient client)
    {
        throw new NotImplementedException();
    }
}
