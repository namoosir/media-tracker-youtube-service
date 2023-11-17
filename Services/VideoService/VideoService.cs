using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Dtos.Video;
using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;
using static MediaTrackerYoutubeService.Constants;

namespace MediaTrackerYoutubeService.Services.VideoService;

public class VideoService : IVideoService
{
    private readonly AppDbContext _context;

    public VideoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Video>>> GetVideos(List<string> videoIds)
    {
        try
        {
            var videoRecords = await _context.Videos
                .Where(video => videoIds.Contains(video.YoutubeId))
                .ToListAsync();
            return ServiceResponse<List<Video>>.Build(videoRecords, true, null);
        }
        catch (Exception e)
        {
            return ServiceResponse<List<Video>>.Build(null, false, e.Message);
        }
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

    public async Task<ServiceResponse<List<Video>>> UpdateVideo(List<UpdateVideoDto> videos)
    {
        var serviceResponse = new ServiceResponse<List<Video>> { Data = new List<Video>() };

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

                if (video.Title != null)
                    foundVideo.Title = video.Title;
                if (video.ViewCount != null)
                    foundVideo.ViewCount = video.ViewCount;
                if (video.LikeCount != null)
                    foundVideo.LikeCount = video.LikeCount;
                if (video.CommentCount != null)
                    foundVideo.CommentCount = video.CommentCount;
                if (video.ThumbnailUrl != null)
                    foundVideo.ThumbnailUrl = video.ThumbnailUrl;
                if (video.ETag != null)
                    foundVideo.ETag = video.ETag;
                if (video.Imported != null)
                    foundVideo.Imported = (bool)video.Imported;

                await _context.SaveChangesAsync();
                serviceResponse.Data.Add(foundVideo);
            }
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

    public async Task<ServiceResponse<List<string>>> VideosNotFound(List<string> videos)
    {
        var serviceResponse = new ServiceResponse<List<string>> { Data = new List<string>() };

        try
        {
            foreach (var video in videos)
            {
                var foundVideo = await _context.Videos.FirstOrDefaultAsync(
                    v => v.YoutubeId == video
                );

                if (foundVideo == null)
                {
                    serviceResponse.Data.Add(video);
                }
            }
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<string>>> GetPendingVideos()
    {
        var serviceResponse = new ServiceResponse<List<string>>();

        try
        {
            serviceResponse.Data = await _context.Videos
                .Where(v => v.Imported == false)
                .Select(v => v.YoutubeId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<string>>> GetOutOfDateVideos()
    {
        var serviceResponse = new ServiceResponse<List<string>>();

        try
        {
            var cutoffTimestamp = DateTime.Now.AddMinutes(-RESOURCE_REFRESH_DELAY);
            serviceResponse.Data = await _context.Videos
                .Where(v => v.UpdatedAt < cutoffTimestamp)
                .Select(v => v.YoutubeId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }
}
