using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Utils;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Utils.Youtube;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Services.UserService;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Services.FetchYoutubeDataService;
using MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;
using MediaTrackerYoutubeService.Services.StoreYoutubeDataService;
using Microsoft.IdentityModel.Tokens;
using MediaTrackerYoutubeService.Services.ChannelService;
using MediaTrackerYoutubeService.Services.VideoService;
using MediaTrackerYoutubeService.Services.PlaylistService;
using MediaTrackerYoutubeService.Dtos.User;
using MediaTrackerYoutubeService.Dtos.Playlist;

namespace MediaTrackerYoutubeService.Services.DataSynchronizationService;

public class DataSynchronizationService : IDataSynchronizationService
{
    private readonly IAuthTokenExchangeService _authTokenExchangeService;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IFetchYoutubeDataService _fetchYoutubeDataService;
    private readonly IProcessYoutubeDataService _processYoutubeDataService;

    private readonly IStoreYoutubeDataService _storeYoutubeDataService;

    private readonly IChannelService _channelService;

    private readonly IVideoService _videoService;

    private readonly IPlaylistService _playlistService;

    private const double REFRESH_DELAY = 10.0;

    public DataSynchronizationService(
        IAuthTokenExchangeService authTokenExchangeService,
        IUserService userService,
        IConfiguration configuration,
        IFetchYoutubeDataService fetchYoutubeDataService,
        IProcessYoutubeDataService processYoutubeDataService,
        IStoreYoutubeDataService storeYoutubeDataService,
        IChannelService channelService,
        IVideoService videoService,
        IPlaylistService playlistService
    )
    {
        _authTokenExchangeService = authTokenExchangeService;
        _userService = userService;
        _configuration = configuration;
        _fetchYoutubeDataService = fetchYoutubeDataService;
        _processYoutubeDataService = processYoutubeDataService;
        _storeYoutubeDataService = storeYoutubeDataService;
        _channelService = channelService;
        _videoService = videoService;
        _playlistService = playlistService;
    }

    public async Task<ServiceResponse<string>> ImportVideoAndChannelStatistics(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<string>> SyncData(int userId)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            if (!(await _userService.UpsertUser(userId)).Success)
                throw new Exception("Failed to add new user or confirm exisiting user");

            // //get user
            // var foundUser = await _userService.GetUser(userId);
            // if (!foundUser.Success)
            //     throw new Exception("Failed to get user");

            // //check time
            // if ((DateTime.Now - foundUser.Data.UpdatedAt).TotalMinutes < REFRESH_DELAY)
            // {
            //     serviceResponse.Data = "Didn't perform sync, not enough time has passed since the last sync";
            //     return serviceResponse;
            // }

            var tokenResult = await _authTokenExchangeService.YoutubeAuthTokenExchange(userId);

            if (!tokenResult.Success)
                throw new Exception("Failed to get youtube token");

            var access_token = tokenResult.Data;
            var apiKey =
                _configuration["YoutubeAPIKey"] ?? throw new Exception("Missing ApiKey in Config");

            YoutubeAPIClient client = new YoutubeAPIClient(access_token, apiKey);

            Console.WriteLine(access_token);

            await Task.WhenAll(
                SyncPlaylistsAndAssociatedVideos(userId, client)
            // SyncSubscriptionsAndAssociatedChannels(userId, client)
            // SyncLikedAndDislikedVideos(userId, client)
            );

            // user.UpdateAt = DateNow



            serviceResponse.Data = "Synced successfully!";
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
            Console.WriteLine(e);
        }
        return serviceResponse;
    }

    private async Task SyncPlaylistsAndAssociatedVideos(int userId, YoutubeAPIClient client)
    {
        var getUserResponse = await _userService.GetUser(userId);
        if (!getUserResponse.Success)
            throw new Exception("Failed to get User");
        var user = getUserResponse!.Data;

        var fetchExternalPlaylistsResult = await _fetchYoutubeDataService.FetchExternalPlaylists(
            user.PlaylistsEtag,
            client
        );
        if (!fetchExternalPlaylistsResult.Success)
            throw new Exception(fetchExternalPlaylistsResult.Message);
        var (playlistsExternal, playlistsEtag) = fetchExternalPlaylistsResult.Data;
        if (playlistsEtag == user.PlaylistsEtag)
            return;

        var notFoundInternalPlaylistsResponse =
            await _processYoutubeDataService.InternalPlaylistsNotFound(playlistsExternal);
        if (!notFoundInternalPlaylistsResponse.Success)
            throw new Exception("Something went wrong");
        var notFoundInternalPlaylists = notFoundInternalPlaylistsResponse.Data;

        var playlistsToInsert = _processYoutubeDataService
            .MapPlaylistResourceToModel(notFoundInternalPlaylists, user)
            .Data;

        await _storeYoutubeDataService.StorePlaylists(playlistsToInsert);

        var playlistsExternalIds = playlistsExternal.Select(playlist => playlist.id).ToList();
        var playlistsExternalModelsResponse = await _playlistService.GetPlaylist(
            playlistsExternalIds
        );
        if (!playlistsExternalModelsResponse.Success)
            throw new Exception("Failed to get playlists");

        var playlistsExternalModels = playlistsExternalModelsResponse.Data;

        var updateUserDto = new UpdateUserDto
        {
            UserId = userId,
            PlaylistsEtag = playlistsEtag,
            VideoPlaylists = playlistsExternalModels
        };

        await _userService.UpdateUser(updateUserDto);

        var playlistsExternalModelsWithDifferentEtags = playlistsExternalModels
            .Where(model =>
            {
                var dual = playlistsExternal.First(playlist => playlist.id == model.YoutubeId);
                if (dual.etag != model.ETag)
                {
                    model.ETag = dual.etag;
                    return true;
                }
                return false;
            })
            .ToList();

        playlistsExternalModelsWithDifferentEtags.AddRange(playlistsToInsert);

        await SyncPlaylistVideos(playlistsExternalModelsWithDifferentEtags, client);
    }

    private async Task SyncPlaylistVideos(List<Playlist> userPlaylists, YoutubeAPIClient client)
    {
        foreach (Playlist playlist in userPlaylists)
        {
            // playlistVideosExteral = fetch
            // compute videosNotFoundInternally
            // Import their Channels if not in INternally
            // Import them add new Row
            // Playlist.upsert relationship(playlistVideosExtneral)
            // Or Playlists.Videos = playlistVideosExtneral


            // List<Video> playlistVideosInternal = playlist.Videos.ToList();
            // Dictionary<string, Video> playlistVideosInternalHashMap = playlistVideosInternal.ToDictionary(video => video.YoutubeId);

            List<Resource> playlistVideosExternal = (
                await _fetchYoutubeDataService.FetchExternalPlaylistVideos(
                    client,
                    playlist.YoutubeId
                )
            ).Data;
            playlistVideosExternal.ForEach(video => video.id = video.contentDetails.videoId);
            List<string> playlistVideoIds = playlistVideosExternal
                .Select(resource => resource.contentDetails.videoId)
                .ToList();

            List<Resource> videosToImportResource = (
                await _processYoutubeDataService.InternalVideosNotFound(playlistVideosExternal)
            ).Data;

            List<Channel> videosToImportContentCreators = await SyncVideoContentCreatorChannels(
                videosToImportResource,
                client
            );

            List<Video> videosToInsert = _processYoutubeDataService
                .MapVideoResourceToModel(videosToImportResource, videosToImportContentCreators)
                .Data;
            //
            await _storeYoutubeDataService.StoreVideos(videosToInsert);

            // Set playlist-video relationship

            var playlistVideos = (await _videoService.GetVideos(playlistVideoIds)).Data;

            var updatePlaylistDto = new UpdatePlaylistDto
            {
                YoutubeId = playlist.YoutubeId,
                ETag = playlist.ETag,
                Videos = playlistVideos,
            };

            await _playlistService.UpdatePlaylist(
                new List<UpdatePlaylistDto> { updatePlaylistDto }
            );
        }
    }

    private async Task<List<Channel>> SyncVideoContentCreatorChannels(
        List<Resource> videosExternal,
        YoutubeAPIClient client
    )
    {
        if (videosExternal.Count == 0)
            return new List<Channel>();

        List<(string channelId, string channelTitle)> channelsExternal = new();

        if (videosExternal[0].snippet.videoOwnerChannelId is null)
        {
            channelsExternal = videosExternal
                .Select(v => (v.snippet.channelId, v.snippet.title))
                .Distinct()
                .ToList();
        }
        else
        {
            channelsExternal = videosExternal
                .Select(v => (v.snippet.videoOwnerChannelId, v.snippet.videoOwnerChannelTitle))
                .Distinct()
                .ToList();
        }

        var channelsToInsert = (
            await _processYoutubeDataService.ProcessPlaylistContentCreatorChannels(channelsExternal)
        ).Data;
        await _storeYoutubeDataService.StoreChannels(channelsToInsert);

        // List Of ContentCreators for ExternalVideos
        return (
            await _channelService.GetChannel(channelsExternal.Select(c => c.channelId).ToList())
        ).Data;
    }

    private async Task SyncSubscriptionsAndAssociatedChannels(int userId, YoutubeAPIClient client)
    {
        throw new NotImplementedException();
    }

    private async Task SyncLikedAndDislikedVideos(int userId, YoutubeAPIClient client)
    {
        UpdateUserDto updateUserDto = new() { UserId = userId };

        var likedVideosExternal = await _fetchYoutubeDataService.FetchExternalLikedVideos(client);
        var dislikedVideosExternal = await _fetchYoutubeDataService.FetchExternalDislikedVideos(
            client
        );

        if (!likedVideosExternal.Success || !dislikedVideosExternal.Success)
            throw new Exception("Failed to fetch videos from youtube");

        if (likedVideosExternal.Data.items.Count != 0)
        {
            var notFoundLikedVideos = await _processYoutubeDataService.InternalVideosNotFound(
                likedVideosExternal.Data.items
            );
            if (!notFoundLikedVideos.Success)
                throw new Exception("Something went wrong");

            if (notFoundLikedVideos.Data.Count > 0)
            {
                var likedVideoChannels = await SyncVideoContentCreatorChannels(
                    notFoundLikedVideos.Data,
                    client
                );

                var likedVideosToInsert = _processYoutubeDataService
                    .MapVideoResourceToModel(notFoundLikedVideos.Data, likedVideoChannels)
                    .Data;

                await _storeYoutubeDataService.StoreVideos(likedVideosToInsert);
            }

            var allExternalLikedVideoIds = likedVideosExternal.Data.items
                .Select(x => x.id)
                .ToList();

            var allExternalLikedVideos = await _videoService.GetVideos(allExternalLikedVideoIds);

            if (!allExternalLikedVideos.Success)
                throw new Exception("Failed to fetch videos from db");

            updateUserDto.LikedVideos = allExternalLikedVideos.Data;
        }

        if (dislikedVideosExternal.Data.items.Count != 0)
        {
            var notFoundDislikedVideos = await _processYoutubeDataService.InternalVideosNotFound(
                dislikedVideosExternal.Data.items
            );
            if (!notFoundDislikedVideos.Success)
                throw new Exception("Something went wrong");

            if (notFoundDislikedVideos.Data.Count > 0)
            {
                var dislikedVideoChannels = await SyncVideoContentCreatorChannels(
                    notFoundDislikedVideos.Data,
                    client
                );

                var dislikedVideosToInsert = _processYoutubeDataService
                    .MapVideoResourceToModel(notFoundDislikedVideos.Data, dislikedVideoChannels)
                    .Data;

                await _storeYoutubeDataService.StoreVideos(dislikedVideosToInsert);
            }

            var allExternalDislikedVideoIds = dislikedVideosExternal.Data.items
                .Select(x => x.id)
                .ToList();

            var allExternalDislikedVideos = await _videoService.GetVideos(
                allExternalDislikedVideoIds
            );

            if (!allExternalDislikedVideos.Success)
                throw new Exception("Failed to fetch videos from db");

            updateUserDto.DislikedVideos = allExternalDislikedVideos.Data;
        }

        updateUserDto.LikedVideosEtag = likedVideosExternal.Data.etag;
        updateUserDto.DislikedVideosEtag = dislikedVideosExternal.Data.etag;
        updateUserDto.UpdatedAt = DateTime.Now;

        await _userService.UpdateUser(updateUserDto);
    }
}

//TODO UPDATE API CLIENT TO TAKE ETAG, REMOVE STATISTIC WHERE NOT NEEDED
