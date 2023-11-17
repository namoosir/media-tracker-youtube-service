using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using MediaTrackerYoutubeService.Utils.Youtube;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public interface IFetchYoutubeDataService
{
    Task<ServiceResponse<(List<Resource> items, string etag)>> FetchExternalPlaylists(
        string etag,
        YoutubeAPIClient client
    );
    Task<ServiceResponse<List<Resource>>> FetchExternalPlaylistVideos(
        YoutubeAPIClient client,
        string playlistId
    );

    Task<ServiceResponse<List<Resource>>> FetchChannels(
        YoutubeAPIClient client,
        List<Channel> channelsToUpdate
    );

    Task<ServiceResponse<(List<Resource> items, string etag)>> FetchExternalLikedVideos(
        YoutubeAPIClient client
    );
    Task<ServiceResponse<(List<Resource> items, string etag)>> FetchExternalDislikedVideos(
        YoutubeAPIClient client
    );
}
