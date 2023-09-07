using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;

namespace MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;

public interface IProcessYoutubeDataService
{
    Task<ServiceResponse<List<UserVideo>>> ProcessYoutubeData(
        List<UserYoutubeVideo> userYoutubeVideos
    );
}
