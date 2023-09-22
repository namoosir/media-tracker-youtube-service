using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.AuthTokenExchangeService;

public interface IAuthTokenExchangeService
{
    Task<ServiceResponse<UserInformation>> YoutubeAuthTokenExchange(int userId);
}
