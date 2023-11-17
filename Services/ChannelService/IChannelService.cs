using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Dtos.Channel;

namespace MediaTrackerYoutubeService.Services.ChannelService;

public interface IChannelService
{
    Task<ServiceResponse<Channel>> GetChannel(string channelId);
    Task<ServiceResponse<List<Channel>>> GetChannel(List<string> channelsId);
    Task<ServiceResponse<List<Channel>>> AddChannel(List<Channel> channels);
    Task<ServiceResponse<List<Channel>>> UpdateChannel(List<UpdateChannelDto> channels);
    Task<ServiceResponse<List<Channel>>> DeleteChannel(List<Channel> channels);
    Task<ServiceResponse<List<string>>> GetPendingChannels();
    Task<ServiceResponse<List<string>>> GetOutOfDateChannels();
}
