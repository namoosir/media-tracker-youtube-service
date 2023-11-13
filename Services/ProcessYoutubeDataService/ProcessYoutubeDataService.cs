using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Services.ChannelService;
using Microsoft.AspNetCore.Components.Web;

namespace MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;

public class ProcessYoutubeDataService : IProcessYoutubeDataService
{
    private readonly IChannelService _channelService;

    public ProcessYoutubeDataService(IChannelService channelService)
    {
        _channelService = channelService;
    }

    private const double REFRESH_DELAY = 10.0;

    public ServiceResponse<(
        List<Playlist>,
        List<Playlist>,
        List<Playlist>
    )> ProcessYoutubePlaylists(
        List<Resource> playlistsExternal,
        List<Playlist> playlistsInternal,
        User user
    )
    {
        List<Playlist> playlistsToUpdate = new List<Playlist>();
        List<Playlist> playlistsToInsert = new List<Playlist>();
        List<Playlist> playlistsToDelete = new List<Playlist>();

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
                playlistsToUpdate.Add(playlistRecord);
            }
            //add updated element to playlistsToUpdate
            else if (
                (DateTime.Now - foundInternal.UpdatedAt).TotalMinutes >= REFRESH_DELAY
                && foundInternal.ETag != pe.etag
            )
            {
                foundInternal.ETag = pe.etag;
                foundInternal.Title = pe.snippet.title;
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

        return ServiceResponse<(List<Playlist>, List<Playlist>, List<Playlist>)>.Build(
            (playlistsToInsert, playlistsToUpdate, playlistsToDelete),
            true,
            null
        );
    }

    public async Task<
        ServiceResponse<(List<Channel>, List<Channel>)>
    > ProcessPlaylistContentCreatorChannels(List<string> channelIdsExternal)
    {
        List<Channel> channelsToInsert = new List<Channel>();
        List<Channel> channelsToUpdate = new List<Channel>();

        foreach (string channelId in channelIdsExternal)
        {
            var internalChannel = (await _channelService.GetChannel(channelId)).Data;

            if (internalChannel == null)
            {
                var newChannel = new Channel
                {
                    YoutubeId = channelId,
                    ETag = "",
                    Videos = new List<Video>(),
                    Title = "",
                    UserSubscribers = new List<User>()
                };

                channelsToInsert.Add(newChannel);
                channelsToUpdate.Add(newChannel);
            }
            else if ((DateTime.Now - internalChannel.UpdatedAt).TotalMinutes >= REFRESH_DELAY)
            {
                channelsToUpdate.Add(internalChannel);
            }
        }

        return ServiceResponse<(List<Channel>, List<Channel>)>.Build(
            (channelsToInsert, channelsToUpdate),
            true,
            null
        );
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

    public ServiceResponse<(List<Video>, List<Video>, List<Video>)> ProcessYoutubePlaylistItems(
        List<Resource> videosExternal,
        List<Video> videosInternal,
        Playlist playlist,
        List<Channel> playlistVideoContentCreatorChannels
    )
    {
        Dictionary<string, Resource> videosExternalHashMap = videosExternal.ToDictionary(
            video => video.id
        );
        Dictionary<string, Video> videosInternalHashMap = videosInternal.ToDictionary(
            video => video.YoutubeId
        );
        Dictionary<string, Channel> playlistVideoContentCreatorChannelsHashMap =
            playlistVideoContentCreatorChannels.ToDictionary(channel => channel.YoutubeId);

        // ToInsert
        List<Resource> videosToInsertResource = videosExternal
            .Where(videosExternal => !videosInternalHashMap.ContainsKey(videosExternal.id))
            .ToList();
        List<Video> videosToInsertModel = videosToInsertResource
            .Select(videoResource =>
            {
                var videoPlaylist = new List<Playlist> { playlist };
                var found = playlistVideoContentCreatorChannelsHashMap.TryGetValue(
                    videoResource.snippet.videoOwnerChannelId,
                    out Channel contentCreatorChannelOfVideo
                );
                return new Video
                {
                    Playlist = videoPlaylist,
                    Channel = contentCreatorChannelOfVideo,
                    YoutubeId = videoResource.id,
                    ETag = videoResource.etag,
                    Title = videoResource.snippet.title,
                    ThumbnailUrl = videoResource.snippet.thumbnails.Default.url ?? ""
                };
            })
            .ToList();

        // ToUpdate
        List<Video> markedVideosToUpdateModel = videosInternal
            .Where(videoInternal =>
            {
                // if ((DateTime.Now - videoInternal.UpdatedAt).TotalMinutes >= REFRESH_DELAY) return false;
                if (
                    !videosExternalHashMap.TryGetValue(
                        videoInternal.YoutubeId,
                        out Resource videoExternalResource
                    )
                )
                    return false;
                if (videoInternal.ETag == videoExternalResource.etag)
                    return false;
                return true;
            })
            .ToList();

        List<Video> videosToUpdateModelWithUpdatedFields = markedVideosToUpdateModel
            .Select(videoInternal =>
            {
                videosExternalHashMap.TryGetValue(
                    videoInternal.YoutubeId,
                    out Resource videoExternalResource
                );

                videoInternal.ETag = videoExternalResource.etag;
                videoInternal.ThumbnailUrl =
                    videoExternalResource.snippet.thumbnails.Default.url ?? "";
                videoInternal.Title = videoExternalResource.snippet.title;

                return videoInternal;
            })
            .ToList();

        // ToDelete
        List<Video> videosToDelete = videosInternal
            .Where(videoInternal => !videosExternalHashMap.ContainsKey(videoInternal.YoutubeId))
            .ToList();

        return ServiceResponse<(List<Video>, List<Video>, List<Video>)>.Build(
            (videosToInsertModel, videosToUpdateModelWithUpdatedFields, videosToDelete),
            true,
            null
        );
    }
}
