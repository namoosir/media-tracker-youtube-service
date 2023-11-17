using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.DataSynchronizationService;

public interface IDataSynchronizationService
{
    Task<ServiceResponse<string>> SyncData(int userId);
}
