using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.AuthTokenExchangeService;

public interface IAuthTokenExchangeService
{
    Task<ServiceResponse<UserInformation>> YoutubeAuthTokenExchange(int userId);
}
