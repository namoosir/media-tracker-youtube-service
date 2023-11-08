using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.ChannelService;

public interface IChannelService
{
    Task<ServiceResponse<List<Channel>>> GetChannel();
    Task<ServiceResponse<List<Channel>>> AddChannel(List<Channel> channels);
    Task<ServiceResponse<List<Channel>>> UpdateChannel(List<Channel> channels);
    Task<ServiceResponse<List<Channel>>> DeleteChannel(List<Channel> channels);
}