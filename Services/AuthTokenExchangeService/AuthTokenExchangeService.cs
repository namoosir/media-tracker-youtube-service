using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.AuthTokenExchangeService;

public class AuthTokenExchangeService : IAuthTokenExchangeService
{
    private readonly HttpClient _httpClient;

    public AuthTokenExchangeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    Task<ServiceResponse<UserInformation>> IAuthTokenExchangeService.YoutubeAuthTokenExchange(
        int userId
    )
    {
        //TODO: exchange that shit cuzzo
        throw new NotImplementedException();
    }
}
