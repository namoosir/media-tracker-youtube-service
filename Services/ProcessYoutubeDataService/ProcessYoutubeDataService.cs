using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Services.ChannelService;
using MediaTrackerYoutubeService.Services.PlaylistService;
using MediaTrackerYoutubeService.Services.VideoService;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;

namespace MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;

public class ProcessYoutubeDataService : IProcessYoutubeDataService
{
    private readonly IChannelService _channelService;
    private readonly IVideoService _videoService;

    private readonly IPlaylistService _playlistService;

    public ProcessYoutubeDataService(
        IChannelService channelService,
        IVideoService videoService,
        IPlaylistService playlistService
    )
    {
        _channelService = channelService;
        _videoService = videoService;
        _playlistService = playlistService;
    }

    private const double REFRESH_DELAY = 10.0;

    public ServiceResponse<List<Playlist>> ProcessYoutubePlaylists(
        List<Resource> playlistsExternal,
        List<Playlist> playlistsInternal,
        User user
    )
    {
        List<Playlist> playlistsToInsert = new List<Playlist>();

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
                    Title = pe.snippet.title,
                    Videos = new List<Video>()
                };

                playlistsToInsert.Add(playlistRecord);
            }
        }

        return ServiceResponse<List<Playlist>>.Build(playlistsToInsert, true, null);
    }

    public async Task<ServiceResponse<List<Channel>>> ProcessPlaylistContentCreatorChannels(
        List<(string channelId, string channelTitle)> channelsExternal
    )
    {
        List<Channel> channelsToInsert = new List<Channel>();

        foreach (var channel in channelsExternal)
        {
            var internalChannel = (await _channelService.GetChannel(channel.channelId)).Data;

            if (internalChannel == null)
            {
                // Check if we have channelI

                var newChannel = new Channel
                {
                    YoutubeId = channel.channelId,
                    ETag = "",
                    Videos = new List<Video>(),
                    Title = channel.channelTitle,
                    UserSubscribers = new List<User>(),
                    Imported = false
                };
                channelsToInsert.Add(newChannel);
            }
        }

        return ServiceResponse<List<Channel>>.Build(channelsToInsert, true, null);
    }

    public ServiceResponse<List<Channel>> FillOutChannelFieldsInInternalModel(
        List<Channel> channelsModel,
        List<Resource> channelsResource
    )
    {
        Dictionary<string, Resource> channelsResourceHashMap = channelsResource.ToDictionary(
            resource => resource.id
        );

        foreach (Channel channelModel in channelsModel)
        {
            channelsResourceHashMap.TryGetValue(
                channelModel.YoutubeId,
                out Resource channelResource
            );

            channelModel.ETag = channelResource.etag;
            channelModel.Title = channelResource.snippet.title;
            channelModel.SubscriberCount =
                (channelResource.statistics.hiddenSubscriberCount ?? true)
                    ? -1
                    : channelResource.statistics.subscriberCount;
            channelModel.VideoCount = channelResource.statistics.videoCount;
            channelModel.ViewCount = channelResource.statistics.viewCount;
            channelModel.ThumbnailUrl = channelResource.snippet.thumbnails.Default.url ?? "";
        }

        return ServiceResponse<List<Channel>>.Build(channelsModel, true, null);
    }

    // public ServiceResponse<(List<Video>, List<Video>, List<Video>)> ProcessYoutubePlaylistItems(
    //     List<Resource> videosExternal,
    //     Playlist playlist,
    //     List<Channel> playlistVideoContentCreatorChannels
    // )
    // {
    //     Dictionary<string, Resource> videosExternalHashMap = videosExternal.ToDictionary(
    //         video => video.id
    //     );
    //     Dictionary<string, Channel> playlistVideoContentCreatorChannelsHashMap = playlistVideoContentCreatorChannels.ToDictionary(channel => channel.YoutubeId);

    //     // ToInsert
    //     List<Resource> videosToInsertResource = videosExternal
    //         .Where(videosExternal => !videosInternalHashMap.ContainsKey(videosExternal.id))
    //         .ToList();

    //     List<Video> videosToInsertModel = videosToInsertResource
    //         .Select(videoResource =>
    //         {
    //             var videoPlaylist = new List<Playlist> { playlist };
    //             var found = playlistVideoContentCreatorChannelsHashMap.TryGetValue(
    //                 videoResource.snippet.videoOwnerChannelId,
    //                 out Channel contentCreatorChannelOfVideo
    //             );
    //             return new Video
    //             {
    //                 Playlist = videoPlaylist,
    //                 Channel = contentCreatorChannelOfVideo,
    //                 YoutubeId = videoResource.id,
    //                 ETag = videoResource.etag,
    //                 Title = videoResource.snippet.title,
    //                 ThumbnailUrl = videoResource.snippet.thumbnails.Default.url ?? "",
    //                 Imported = false,
    //                 LikedByUsers = new List<User>(),
    //                 DislikedByUsers = new List<User>(),
    //             };
    //         })
    //         .ToList();

    //     // ToUpdate
    //     List<Video> markedVideosToUpdateModel = videosInternal
    //         .Where(videoInternal =>
    //         {
    //             if ((DateTime.Now - videoInternal.UpdatedAt).TotalMinutes >= REFRESH_DELAY) return false;
    //             if (!videosExternalHashMap.TryGetValue(videoInternal.YoutubeId,out Resource videoExternalResource)) return false;
    //             if (videoInternal.ETag == videoExternalResource.etag) return false;
    //             return true;
    //         })
    //         .ToList();

    //     List<Video> videosToUpdateModelWithUpdatedFields = markedVideosToUpdateModel
    //         .Select(videoInternal =>
    //         {
    //             videosExternalHashMap.TryGetValue(
    //                 videoInternal.YoutubeId,
    //                 out Resource videoExternalResource
    //             );

    //             videoInternal.ETag = videoExternalResource.etag;
    //             videoInternal.ThumbnailUrl =
    //                 videoExternalResource.snippet.thumbnails.Default.url ?? "";
    //             videoInternal.Title = videoExternalResource.snippet.title;

    //             return videoInternal;
    //         })
    //         .ToList();

    //     // ToDelete
    //     List<Video> videosToDelete = videosInternal
    //         .Where(videoInternal => !videosExternalHashMap.ContainsKey(videoInternal.YoutubeId))
    //         .ToList();

    //     return ServiceResponse<(List<Video>, List<Video>, List<Video>)>.Build(
    //         (videosToInsertModel, videosToUpdateModelWithUpdatedFields, videosToDelete),
    //         true,
    //         null
    //     );
    // }

    public ServiceResponse<List<Video>> MapVideoResourceToModel(
        List<Resource> videosExternal,
        List<Channel> videoContentCreatorChannels
    )
    {
        Dictionary<string, Channel> videoContentCreatorChannelsHashMap =
            videoContentCreatorChannels.ToDictionary(channel => channel.YoutubeId);

        List<Video> videosToInsert = videosExternal
            .Select(videoResource =>
            {
                Channel contentCreatorChannelOfVideo;
                string YouTubeId;
                if (videoResource.snippet.videoOwnerChannelId == null)
                {
                    contentCreatorChannelOfVideo = videoContentCreatorChannelsHashMap[
                        videoResource.snippet.channelId
                    ];
                    YouTubeId = videoResource.id;
                }
                else
                {
                    contentCreatorChannelOfVideo = videoContentCreatorChannelsHashMap[
                        videoResource.snippet.videoOwnerChannelId
                    ];
                    YouTubeId = videoResource.contentDetails.videoId;
                }

                return new Video
                {
                    Playlist = new List<Playlist>(),
                    Channel = contentCreatorChannelOfVideo,
                    YoutubeId = YouTubeId,
                    ETag = videoResource.etag,
                    Title = videoResource.snippet.title,
                    ThumbnailUrl = videoResource.snippet.thumbnails.Default.url ?? "",
                    Imported = false,
                    LikedByUsers = new List<User>(),
                    DislikedByUsers = new List<User>(),
                };
            })
            .ToList();

        return ServiceResponse<List<Video>>.Build(videosToInsert, true, null);
    }

    public async Task<ServiceResponse<List<Resource>>> InternalVideosNotFound(
        List<Resource> videosExternal
    )
    {
        var serviceResponse = new ServiceResponse<List<Resource>>();

        try
        {
            var videoIds = videosExternal.Select(x => x.id).ToList();
            var videoServiceResponse = await _videoService.VideosNotFound(videoIds);

            if (!videoServiceResponse.Success)
                throw new Exception("Something went wrong");

            var videoIdsToInsert = videoServiceResponse.Data;

            var resourcesToInsert = videosExternal.Where(x => videoIdsToInsert.Contains(x.id));
            serviceResponse.Data = resourcesToInsert.ToList();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public ServiceResponse<List<Playlist>> MapPlaylistResourceToModel(
        List<Resource> playlistsExternal,
        User userPlaylistOwner
    )
    {
        List<Playlist> playlistsToInsert = playlistsExternal
            .Select(playlistResource =>
            {
                return new Playlist
                {
                    User = userPlaylistOwner,
                    YoutubeId = playlistResource.id,
                    ETag = playlistResource.etag,
                    Title = playlistResource.snippet.title,
                    Videos = new List<Video>()
                };
            })
            .ToList();

        return ServiceResponse<List<Playlist>>.Build(playlistsToInsert, true, null);
    }

    public async Task<ServiceResponse<List<Resource>>> InternalPlaylistsNotFound(
        List<Resource> playlistsExternal
    )
    {
        var serviceResponse = new ServiceResponse<List<Resource>>();

        try
        {
            var playlistIds = playlistsExternal.Select(x => x.id).ToList();
            var playlistServiceResponse = await _playlistService.PlaylistsNotFound(playlistIds);

            if (!playlistServiceResponse.Success)
                throw new Exception("Something went wrong");

            var playlistIdsToInsert = playlistServiceResponse.Data;

            var resourcesToInsert = playlistsExternal.Where(
                x => playlistIdsToInsert.Contains(x.id)
            );
            serviceResponse.Data = resourcesToInsert.ToList();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}
