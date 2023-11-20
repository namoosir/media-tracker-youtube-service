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
using static MediaTrackerYoutubeService.Utils.ServiceResponseUtils;
using MediaTrackerYoutubeService.Dtos.Video;
using MediaTrackerYoutubeService.Dtos.Channel;
using static MediaTrackerYoutubeService.Utils.Youtube.YoutubeAPIClient;
using static MediaTrackerYoutubeService.Constants;

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

    public async Task<ServiceResponse<string>> SyncData(int userId)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            User user = TryGetThrow(
                await _userService.UpsertUser(userId),
                "Failed to add new user or confirm exisiting user"
            );

            //check time
            if ((DateTime.Now - user.UpdatedAt).TotalMinutes < USER_REFRESH_DELAY)
            {
                serviceResponse.Data =
                    "Didn't perform sync, not enough time has passed since the last sync for the user";
                return serviceResponse;
            }

            var access_token = TryGetThrow(
                await _authTokenExchangeService.YoutubeAuthTokenExchange(userId),
                "Failed to get youtube token"
            );

            var apiKey =
                _configuration["YoutubeAPIKey"] ?? throw new Exception("Missing ApiKey in Config");

            YoutubeAPIClient client = new YoutubeAPIClient(access_token, apiKey);

            Console.WriteLine(access_token);

            // Issue with SharedContext. Need Seperate Contexts for each
            // await Task.WhenAll(
            //     SyncPlaylistsAndAssociatedVideos(userId, client),

            //     SyncLikedAndDislikedVideos(userId, client)
            // );

            await SyncSubscriptionsAndAssociatedChannels(user, client);
            await SyncPlaylistsAndAssociatedVideos(user, client);
            await SyncLikedAndDislikedVideos(user, client);
            await ImportPendingVideosAndChannelStatistics(client);
            await ImportVideoAndChannelStatistics(client);

            var userToUpdate = new UpdateUserDto { UserId = user.UserId, UpdatedAt = DateTime.Now };

            await _userService.UpdateUser(userToUpdate);

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

    public async Task<ServiceResponse<string>> ImportVideoAndChannelStatistics(
        YoutubeAPIClient client
    )
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            var videosToUpdate = new List<UpdateVideoDto>();
            var channelsToUpdate = new List<UpdateChannelDto>();

            var outOfDateVideos = TryGetThrow(await _videoService.GetOutOfDateVideos());
            var outOfDateChannels = TryGetThrow(await _channelService.GetOutOfDateChannels());
            var externalOutOfDateVideos = TryGetThrow(
                await _fetchYoutubeDataService.FetchVideos(client, outOfDateVideos)
            );
            var externalOutOfDateChannels = TryGetThrow(
                await _fetchYoutubeDataService.FetchChannels(client, outOfDateChannels)
            );

            var videoIdToResource = externalOutOfDateVideos.ToDictionary(resource => resource.id);

            var channelIdToResource = externalOutOfDateChannels.ToDictionary(
                resource => resource.id
            );

            outOfDateVideos.ForEach(id =>
            {
                var resource = videoIdToResource[id];
                var videoDto = new UpdateVideoDto
                {
                    YoutubeId = id,
                    Title = resource.snippet.title,
                    ViewCount = resource.statistics.viewCount,
                    LikeCount = resource.statistics.likeCount,
                    CommentCount = resource.statistics.commentCount,
                    ThumbnailUrl = resource.snippet.thumbnails.Default.url,
                    Imported = true
                };
                videosToUpdate.Add(videoDto);
            });

            outOfDateChannels.ForEach(id =>
            {
                var resource = channelIdToResource[id];
                var channelDto = new UpdateChannelDto
                {
                    YoutubeId = id,
                    Title = resource.snippet.title,
                    SubscriberCount = resource.statistics.subscriberCount,
                    ViewCount = resource.statistics.viewCount,
                    VideoCount = resource.statistics.videoCount,
                    ThumbnailUrl = resource.snippet.thumbnails.Default.url,
                    Imported = true
                };
                channelsToUpdate.Add(channelDto);
            });

            await _videoService.UpdateVideo(videosToUpdate);
            await _channelService.UpdateChannel(channelsToUpdate);

            serviceResponse.Data = "Success";
        }
        catch (Exception e)
        {
            serviceResponse.Success = true;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<string>> ImportPendingVideosAndChannelStatistics(
        YoutubeAPIClient client
    )
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            var videosToUpdate = new List<UpdateVideoDto>();
            var channelsToUpdate = new List<UpdateChannelDto>();

            var pendingVideos = TryGetThrow(await _videoService.GetPendingVideos());
            var pendingChannels = TryGetThrow(await _channelService.GetPendingChannels());

            var externalPendingVideos = TryGetThrow(
                await _fetchYoutubeDataService.FetchVideos(client, pendingVideos)
            );
            var externalPendingChannels = TryGetThrow(
                await _fetchYoutubeDataService.FetchChannels(client, pendingChannels)
            );

            Dictionary<string, Resource> videoIdToResource = externalPendingVideos.ToDictionary(
                resource => resource.id
            );

            Dictionary<string, Resource> channelIdToResource = externalPendingChannels.ToDictionary(
                resource => resource.id
            );

            pendingVideos.ForEach(id =>
            {
                var resource = videoIdToResource[id];
                var videoDto = new UpdateVideoDto
                {
                    YoutubeId = id,
                    Title = resource.snippet.title,
                    ViewCount = resource.statistics.viewCount,
                    LikeCount = resource.statistics.likeCount,
                    CommentCount = resource.statistics.commentCount,
                    ThumbnailUrl = resource.snippet.thumbnails.Default.url,
                    Imported = true
                };
                videosToUpdate.Add(videoDto);
            });

            pendingChannels.ForEach(id =>
            {
                var resource = channelIdToResource[id];
                var channelDto = new UpdateChannelDto
                {
                    YoutubeId = id,
                    Title = resource.snippet.title,
                    SubscriberCount = resource.statistics.subscriberCount,
                    ViewCount = resource.statistics.viewCount,
                    VideoCount = resource.statistics.videoCount,
                    ThumbnailUrl = resource.snippet.thumbnails.Default.url,
                    Imported = true
                };
                channelsToUpdate.Add(channelDto);
            });
            await _videoService.UpdateVideo(videosToUpdate);
            await _channelService.UpdateChannel(channelsToUpdate);

            serviceResponse.Data = "Success!";
        }
        catch (Exception e)
        {
            serviceResponse.Success = true;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    private async Task SyncPlaylistsAndAssociatedVideos(User user, YoutubeAPIClient client)
    {
        var (playlistsExternal, playlistsEtag) = TryGetThrow(
            await _fetchYoutubeDataService.FetchExternalPlaylists(user.PlaylistsEtag, client)
        );
        if (playlistsEtag == user.PlaylistsEtag)
            return;

        var notFoundInternalPlaylists = TryGetThrow(
            await _processYoutubeDataService.InternalPlaylistsNotFound(playlistsExternal)
        );

        var playlistsToInsert = _processYoutubeDataService
            .MapPlaylistResourceToModel(notFoundInternalPlaylists, user)
            .Data;
        await _storeYoutubeDataService.StorePlaylists(playlistsToInsert);

        var playlistsExternalIds = playlistsExternal.Select(playlist => playlist.id).ToList();
        var playlistsExternalModels = TryGetThrow(
            await _playlistService.GetPlaylist(playlistsExternalIds),
            "Failed to get playlists"
        );

        var updateUserDto = new UpdateUserDto
        {
            UserId = user.UserId,
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
            List<Resource> playlistVideosExternal = TryGetThrow(
                await _fetchYoutubeDataService.FetchExternalPlaylistVideos(
                    client,
                    playlist.YoutubeId
                )
            );
            playlistVideosExternal.ForEach(video => video.id = video.contentDetails.videoId);

            await ImportNonInternalVideos(playlistVideosExternal, client);

            List<string> playlistVideoIds = playlistVideosExternal
                .Select(resource => resource.contentDetails.videoId)
                .ToList();

            var playlistVideos = TryGetThrow(await _videoService.GetVideos(playlistVideoIds));

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

        List<(string? channelId, string? channelTitle)> channelsExternal = new();

        if (videosExternal[0].snippet.videoOwnerChannelId is null)
        {
            channelsExternal = videosExternal
                .Select(v =>
                {
                    string channelTitle = v.snippet.channelTitle ?? v.snippet.title;
                    string channelId = v.snippet.resourceId?.channelId ?? v.snippet.channelId;
                    return (channelId, channelTitle);
                })
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

        var channelsToInsert = TryGetThrow(
            await _processYoutubeDataService.ProcessPlaylistContentCreatorChannels(channelsExternal)
        );

        await _storeYoutubeDataService.StoreChannels(channelsToInsert);

        // List Of ContentCreators for ExternalVideos
        return TryGetThrow(
            await _channelService.GetChannel(channelsExternal.Select(c => c.channelId).ToList())
        );
    }

    private async Task SyncSubscriptionsAndAssociatedChannels(User user, YoutubeAPIClient client)
    {
        var (subscriptionsExternal, externalEtag) = TryGetThrow(
            await _fetchYoutubeDataService.FetchSubscriptions(client, user.SubscriptionsEtag)
        );
        if (user.SubscriptionsEtag == externalEtag)
            return;

        var channelsExternal = await SyncVideoContentCreatorChannels(subscriptionsExternal, client);

        UpdateUserDto updateUserDto =
            new()
            {
                UserId = user.UserId,
                SubscribedChannels = channelsExternal,
                SubscriptionsEtag = externalEtag
            };
        await _userService.UpdateUser(updateUserDto);
    }

    private async Task ImportNonInternalVideos(
        List<Resource> externalVideos,
        YoutubeAPIClient client
    )
    {
        var notFoundVideos = TryGetThrow(
            await _processYoutubeDataService.InternalVideosNotFound(externalVideos)
        );

        var videoChannels = await SyncVideoContentCreatorChannels(notFoundVideos, client);

        var videosToInsert = _processYoutubeDataService
            .MapVideoResourceToModel(notFoundVideos, videoChannels)
            .Data;

        await _storeYoutubeDataService.StoreVideos(videosToInsert);
    }

    private async Task SyncLikedAndDislikedVideos(User user, YoutubeAPIClient client)
    {
        UpdateUserDto updateUserDto = new() { UserId = user.UserId };

        foreach (var rating in new Rating[] { Rating.Dislike, Rating.Like })
        {
            string internalEtag =
                (rating == Rating.Like) ? user.LikedVideosEtag : user.DislikedVideosEtag;

            var (ratedVideosExternal, externalEtag) = TryGetThrow(
                await _fetchYoutubeDataService.FetchExternalRatedVideos(
                    client,
                    rating,
                    internalEtag
                )
            );
            if (internalEtag == externalEtag)
                continue;

            await ImportNonInternalVideos(ratedVideosExternal, client);

            var allExternalRatedVideoIds = ratedVideosExternal.Select(x => x.id).ToList();

            if (rating == Rating.Dislike)
            {
                updateUserDto.DislikedVideosEtag = externalEtag;
                updateUserDto.DislikedVideos = TryGetThrow(
                    await _videoService.GetVideos(allExternalRatedVideoIds),
                    "Failed to fetch videos from db"
                );
            }
            if (rating == Rating.Like)
            {
                updateUserDto.LikedVideosEtag = externalEtag;
                updateUserDto.LikedVideos = TryGetThrow(
                    await _videoService.GetVideos(allExternalRatedVideoIds),
                    "Failed to fetch videos from db"
                );
            }
        }

        updateUserDto.UpdatedAt = DateTime.Now;
        await _userService.UpdateUser(updateUserDto);
    }
}
