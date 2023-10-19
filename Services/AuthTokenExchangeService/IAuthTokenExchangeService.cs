using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.AuthTokenExchangeService;

public interface IAuthTokenExchangeService
{
    Task<ServiceResponse<string>> YoutubeAuthTokenExchange(int userId);
}
