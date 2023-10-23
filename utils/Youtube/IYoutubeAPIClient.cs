using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using static MediaTrackerYoutubeService.Utils.Youtube.YoutubeAPIClient;

namespace MediaTrackerYoutubeService.Utils.Youtube;

public interface IYoutubeAPIClient
{
    public Task<YoutubeAPIResponse> GetMyPlaylists();

    public Task<YoutubeAPIResponse> GetMyPlaylistItems(string playlistId);

    public Task<YoutubeAPIResponse> GetRatedVideos(Rating rating);

    public Task<YoutubeAPIResponse> GetSubscriptions();

    public Task<YoutubeAPIResponse> GetVideos(List<string> videoIds);

    public Task<YoutubeAPIResponse> GetChannels(List<string> channelIds);
}
