using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerYoutubeService.Services.ChannelService;

public class ChannelService : IChannelService
{
    private readonly AppDbContext _context;

    public ChannelService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Channel>>> GetChannel()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<List<Channel>>> AddChannel(List<Channel> channels)
    {
        var serviceResponse = new ServiceResponse<List<Channel>>();

        try
        {
            _context.Channels.AddRange(channels);
            await _context.SaveChangesAsync();

            serviceResponse.Data = channels;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Channel>>> UpdateChannel(List<Channel> channels)
    {
        var serviceResponse = new ServiceResponse<List<Channel>>();

        try
        {
            foreach (var channel in channels)
            {
                var foundChannel = await _context.Channels.FirstOrDefaultAsync(
                    c => c.YoutubeId == channel.YoutubeId
                );

                if (foundChannel == null)
                {
                    throw new Exception(
                        $"Failed to update channel with id '{channel.YoutubeId}'. Channel not found."
                    );
                }

                foundChannel.Title = channel.Title;
                foundChannel.ViewCount = channel.ViewCount;
                foundChannel.SubscriberCount = channel.SubscriberCount;
                foundChannel.VideoCount = channel.VideoCount;
                foundChannel.ThumbnailUrl = channel.ThumbnailUrl;
                foundChannel.ETag = channel.ETag;

                await _context.SaveChangesAsync();
            }

            serviceResponse.Data = channels;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Channel>>> DeleteChannel(List<Channel> channels)
    {
        var serviceResponse = new ServiceResponse<List<Channel>>();

        try
        {
            var channelIds = channels.Select(c => c.YoutubeId).ToArray();

            await _context.Channels
                .Where(c => channelIds.Contains(c.YoutubeId))
                .ExecuteDeleteAsync();
            serviceResponse.Data = channels;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}
