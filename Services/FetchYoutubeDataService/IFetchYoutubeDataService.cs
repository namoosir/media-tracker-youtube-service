using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Utils.Youtube;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public interface IFetchYoutubeDataService
{
    Task<ServiceResponse<List<Resource>>> fetchExternalPlaylists(YoutubeAPIClient client);
    Task<ServiceResponse<List<Resource>>> fetchExternalPlaylistVideos(
        YoutubeAPIClient client,
        string playlistId
    );

    Task<ServiceResponse<List<Resource>>> fetchChannels(
        YoutubeAPIClient client,
        List<Channel> channelsToUpdate
    );
}
