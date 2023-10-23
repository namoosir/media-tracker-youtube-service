using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.UserVideoService;

public interface IUserVideoService
{
    Task<ServiceResponse<string>> FetchAndStoreYoutubeDataByUserInformation(
        UserInformation userInformation
    );
}
