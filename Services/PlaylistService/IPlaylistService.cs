using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Dtos.Playlist;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.PlaylistService;

public interface IPlaylistService
{
    Task<ServiceResponse<List<Playlist>>> GetPlaylist(List<string> playlistIds);
    Task<ServiceResponse<List<Playlist>>> AddPlaylist(List<Playlist> playlists);
    Task<ServiceResponse<List<Playlist>>> UpdatePlaylist(List<UpdatePlaylistDto> playlists);
    Task<ServiceResponse<List<Playlist>>> DeletePlaylist(List<Playlist> playlists);
    Task<ServiceResponse<List<string>>> PlaylistsNotFound(List<string> videos);
}
