using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.PlaylistService;

public interface IPlaylistService
{
    Task<ServiceResponse<List<Playlist>>> GetPlaylist();
    Task<ServiceResponse<List<Playlist>>> AddPlaylist(List<Playlist> playlists);
    Task<ServiceResponse<List<Playlist>>> UpdatePlaylist(List<Playlist> playlists);
    Task<ServiceResponse<List<Playlist>>> DeletePlaylist(List<Playlist> playlists);
}
