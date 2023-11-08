using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerYoutubeService.Services.VideoService;

public class VideoService : IVideoService
{
    private readonly AppDbContext _context;

    public VideoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Video>>> GetVideo()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<List<Video>>> AddVideo(List<Video> videos)
    {
        var serviceResponse = new ServiceResponse<List<Video>>();

        try
        {
            _context.Videos.AddRange(videos);
            await _context.SaveChangesAsync();

            serviceResponse.Data = videos;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Video>>> UpdateVideo(List<Video> videos)
    {
        var serviceResponse = new ServiceResponse<List<Video>>();

        try
        {
            foreach (var video in videos)
            {
                var foundVideo = await _context.Videos.FirstOrDefaultAsync(
                    v => v.YoutubeId == video.YoutubeId
                );

                if (foundVideo == null)
                {
                    throw new Exception(
                        $"Failed to update video with id '{video.YoutubeId}'. Video not found."
                    );
                }

                foundVideo.ViewCount = video.ViewCount;
                foundVideo.LikeCount = video.LikeCount;
                foundVideo.CommentCount = video.CommentCount;
                foundVideo.ThumbnailUrl = video.ThumbnailUrl;
                foundVideo.ETag = video.ETag;

                await _context.SaveChangesAsync();
            }

            serviceResponse.Data = videos;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Video>>> DeleteVideo(List<Video> videos)
    {
        var serviceResponse = new ServiceResponse<List<Video>>();

        try
        {
            var videoIds = videos.Select(v => v.YoutubeId).ToArray();

            await _context.Videos.Where(v => videoIds.Contains(v.YoutubeId)).ExecuteDeleteAsync();

            serviceResponse.Data = videos;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}