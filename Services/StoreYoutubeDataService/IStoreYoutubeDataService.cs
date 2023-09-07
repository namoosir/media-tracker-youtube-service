using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.StoreYoutubeDataService;

public interface IStoreYoutubeDataService
{
    Task<ServiceResponse<string>> StoreYoutubeData(List<UserVideo> userVideos);
}
