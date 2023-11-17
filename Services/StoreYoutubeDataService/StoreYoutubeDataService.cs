using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Services.PlaylistService;
using MediaTrackerYoutubeService.Services.ChannelService;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Services.VideoService;

namespace MediaTrackerYoutubeService.Services.StoreYoutubeDataService;

public class StoreYoutubeDataService : IStoreYoutubeDataService
{
    private readonly IPlaylistService _playlistService;
    private readonly IChannelService _channelService;
    private readonly IVideoService _videoService;

    public StoreYoutubeDataService(
        IPlaylistService playlistService,
        IChannelService channelService,
        IVideoService videoService
    )
    {
        _playlistService = playlistService;
        _channelService = channelService;
        _videoService = videoService;
    }

    public async Task<ServiceResponse<string>> StorePlaylists(List<Playlist> playlistsToInsert)
    {
        var response = ServiceResponse<string>.Build("", true, null);
        if (!(await _playlistService.AddPlaylist(playlistsToInsert)).Success)
            response.Success = false;

        return response;
    }

    public async Task<ServiceResponse<string>> StoreChannels(List<Channel> channelsToInsert)
    {
        var response = ServiceResponse<string>.Build("", true, null);
        if (!(await _channelService.AddChannel(channelsToInsert)).Success)
            response.Success = false;

        return response;
    }

    public async Task<ServiceResponse<string>> StoreVideos(List<Video> videosToInsert)
    {
        var response = ServiceResponse<string>.Build("", true, null);
        if (!(await _videoService.AddVideo(videosToInsert)).Success)
            response.Success = false;

        return response;
    }
}
