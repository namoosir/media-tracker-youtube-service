using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;

public interface IProcessYoutubeDataService
{
    ServiceResponse<List<Playlist>> ProcessYoutubePlaylists(
        List<Resource> playlistsExternal,
        List<Playlist> playlistsInternal,
        User user
    );

    ServiceResponse<List<Video>> MapVideoResourceToModel(
        List<Resource> videosExternal,
        List<Channel> videoContentCreatorChannels
    );

    ServiceResponse<List<Playlist>> MapPlaylistResourceToModel(
        List<Resource> playlistsExternal,
        User userPlaylistOwner
    );

    Task<ServiceResponse<List<Channel>>> ProcessPlaylistContentCreatorChannels(
        List<(string channelId, string channelTitle)> channelsExternal
    );

    ServiceResponse<List<Channel>> FillOutChannelFieldsInInternalModel(
        List<Channel> channelsORM,
        List<Resource> channelsResource
    );

    Task<ServiceResponse<List<Resource>>> InternalVideosNotFound(List<Resource> videosExternal);
    Task<ServiceResponse<List<Resource>>> InternalPlaylistsNotFound(
        List<Resource> playlistsExternal
    );
}
