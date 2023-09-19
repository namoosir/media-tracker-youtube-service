using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public interface IFetchYoutubeDataService
{
    Task<ServiceResponse<List<UserYoutubeVideo>>> FetchLikedVideos(UserInformation userInformation);
}