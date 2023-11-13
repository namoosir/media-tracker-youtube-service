using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;

public interface IProcessYoutubeDataService
{
    ServiceResponse<(List<Playlist>, List<Playlist>, List<Playlist>)> ProcessYoutubePlaylists(
        List<Resource> playlistsExternal,
        List<Playlist> playlistsInternal,
        User user
    );

    ServiceResponse<(List<Video>, List<Video>, List<Video>)> ProcessYoutubePlaylistItems(
        List<Resource> videosExternal,
        List<Video> videosInternal,
        Playlist playlist,
        List<Channel> playlistVideoContentCreatorChannels
    );

    Task<ServiceResponse<(List<Channel>, List<Channel>)>> ProcessPlaylistContentCreatorChannels(
        List<string> channelIdsExternal
    );

    ServiceResponse<List<Channel>> FillOutChannelFieldsInInternalModel(
        List<Channel> channelsORM,
        List<Resource> channelsResource
    );
}
