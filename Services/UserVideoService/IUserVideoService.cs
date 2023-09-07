using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.UserVideoService;

public interface IUserVideoService
{
    Task<ServiceResponse<string>> FetchAndStoreYoutubeDataByUserInformation(
        UserInformation userInformation
    );
}
