using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.VideoService;

public interface IVideoService
{
    Task<ServiceResponse<List<Video>>> GetVideo();
    Task<ServiceResponse<List<Video>>> AddVideo(List<Video> videos);
    Task<ServiceResponse<List<Video>>> UpdateVideo(List<Video> videos);
    Task<ServiceResponse<List<Video>>> DeleteVideo(List<Video> videos);
}
