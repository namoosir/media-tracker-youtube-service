using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Dtos.Channel;
using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;
using static MediaTrackerYoutubeService.Constants;

namespace MediaTrackerYoutubeService.Services.ChannelService;

public class ChannelService : IChannelService
{
    private readonly AppDbContext _context;

    public ChannelService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<Channel>> GetChannel(string channelId)
    {
        var serviceResponse = new ServiceResponse<Channel>();
        try
        {
            var channel = await _context.Channels.FirstOrDefaultAsync(
                channel => channel.YoutubeId == channelId
            );
            if (channel is null)
                throw new Exception($"No user with id {channel} exists");
            serviceResponse.Data = channel;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Channel>>> GetChannel(List<string> channelsIds)
    {
        try
        {
            var channelRecords = await _context.Channels
                .Where(channel => channelsIds.Contains(channel.YoutubeId))
                .ToListAsync();
            return ServiceResponse<List<Channel>>.Build(channelRecords, true, null);
        }
        catch (Exception e)
        {
            return ServiceResponse<List<Channel>>.Build(null, false, e.Message);
        }
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

    public async Task<ServiceResponse<List<Channel>>> UpdateChannel(List<UpdateChannelDto> channels)
    {
        var serviceResponse = new ServiceResponse<List<Channel>> { Data = new List<Channel>() };

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

                if (channel.Title != null)
                    foundChannel.Title = channel.Title;
                if (channel.ViewCount != null)
                    foundChannel.ViewCount = channel.ViewCount;
                if (channel.SubscriberCount != null)
                    foundChannel.SubscriberCount = channel.SubscriberCount;
                if (channel.VideoCount != null)
                    foundChannel.VideoCount = channel.VideoCount;
                if (channel.ThumbnailUrl != null)
                    foundChannel.ThumbnailUrl = channel.ThumbnailUrl;
                if (channel.ETag != null)
                    foundChannel.ETag = channel.ETag;
                if (channel.Imported != null)
                    foundChannel.Imported = (bool)channel.Imported;

                await _context.SaveChangesAsync();
                serviceResponse.Data.Add(foundChannel);
            }
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

    public async Task<ServiceResponse<List<string>>> GetPendingChannels()
    {
        var serviceResponse = new ServiceResponse<List<string>>();

        try
        {
            serviceResponse.Data = await _context.Channels
                .Where(c => c.Imported == false)
                .Select(c => c.YoutubeId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<string>>> GetOutOfDateChannels()
    {
        var serviceResponse = new ServiceResponse<List<string>>();

        try
        {
            var cutoffTimestamp = DateTime.Now.AddMinutes(-RESOURCE_REFRESH_DELAY);
            serviceResponse.Data = await _context.Channels
                .Where(c => c.UpdatedAt < cutoffTimestamp)
                .Select(c => c.YoutubeId)
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
