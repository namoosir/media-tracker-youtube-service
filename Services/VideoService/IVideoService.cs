using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Dtos.Video;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.VideoService;

public interface IVideoService
{
    Task<ServiceResponse<List<Video>>> GetVideos(List<string> videoIds);
    Task<ServiceResponse<List<Video>>> AddVideo(List<Video> videos);
    Task<ServiceResponse<List<Video>>> UpdateVideo(List<UpdateVideoDto> videos);
    Task<ServiceResponse<List<Video>>> DeleteVideo(List<Video> videos);
    Task<ServiceResponse<List<string>>> VideosNotFound(List<string> videos);
    Task<ServiceResponse<List<string>>> GetPendingVideos();
    Task<ServiceResponse<List<string>>> GetOutOfDateVideos();
}
