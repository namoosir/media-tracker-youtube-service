using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public class FetchYoutubeDataService : IFetchYoutubeDataService
{
    private readonly HttpClient _httpClient;

    public FetchYoutubeDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ServiceResponse<List<UserYoutubeVideo>>> FetchYoutubeVideos(
        UserInformation userInformation
    )
    {
        throw new NotImplementedException();
    }
}
