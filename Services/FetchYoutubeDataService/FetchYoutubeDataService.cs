using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Services.FetchYoutubeDataService;

public class FetchYoutubeDataService : IFetchYoutubeDataService
{
    private readonly HttpClient _httpClient;

    public FetchYoutubeDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // public async Task<ServiceResponse<List<UserYoutubeVideo>>> FetchLikedVideos(UserInformation userInformation)
    // {
    //     throw new NotImplementedException();
    // }
}
