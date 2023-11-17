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
        List<string> channelIds
    );

    Task<ServiceResponse<(List<Resource> items, string etag)>> FetchSubscriptions(
        YoutubeAPIClient client,
        string internalEtag
    );

    Task<ServiceResponse<(List<Resource> items, string etag)>> FetchExternalRatedVideos(
        YoutubeAPIClient client,
        YoutubeAPIClient.Rating rating,
        string internalEtag
    );
    Task<ServiceResponse<List<Resource>>> FetchVideos(
        YoutubeAPIClient client,
        List<string> videoIds
    );
}
