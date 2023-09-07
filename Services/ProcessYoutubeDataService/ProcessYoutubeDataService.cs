using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;

public class ProcessYoutubeDataService : IProcessYoutubeDataService
{
    public Task<ServiceResponse<List<UserVideo>>> ProcessYoutubeData(
        List<UserYoutubeVideo> userYoutubeVideos
    )
    {
        throw new NotImplementedException();
    }
}
