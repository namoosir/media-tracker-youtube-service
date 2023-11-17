using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.StoreYoutubeDataService;

public interface IStoreYoutubeDataService
{
    Task<ServiceResponse<string>> StorePlaylists(List<Playlist> playlistsToInsert);
    Task<ServiceResponse<string>> StoreVideos(List<Video> videosToInsert);
    Task<ServiceResponse<string>> StoreChannels(List<Channel> channelsToInsert);
}
