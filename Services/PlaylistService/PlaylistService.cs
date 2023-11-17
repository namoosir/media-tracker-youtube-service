using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Services.Utils;
using MediaTrackerYoutubeService.Dtos.Playlist;

namespace MediaTrackerYoutubeService.Services.PlaylistService;

public class PlaylistService : IPlaylistService
{
    private readonly AppDbContext _context;

    public PlaylistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Playlist>>> GetPlaylist(List<string> playlistIds)
    {
        var serviceResponse = new ServiceResponse<List<Playlist>>();

        try
        {
            var playlistRecords = await _context.Playlists
                .Where(playlist => playlistIds.Contains(playlist.YoutubeId))
                .ToListAsync();
            serviceResponse.Data = playlistRecords;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Playlist>>> AddPlaylist(List<Playlist> playlists)
    {
        var serviceResponse = new ServiceResponse<List<Playlist>>();

        try
        {
            _context.Playlists.AddRange(playlists);
            await _context.SaveChangesAsync();

            serviceResponse.Data = playlists;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Playlist>>> UpdatePlaylist(
        List<UpdatePlaylistDto> playlists
    )
    {
        var serviceResponse = new ServiceResponse<List<Playlist>> { Data = new List<Playlist>() };

        try
        {
            foreach (var playlist in playlists)
            {
                var foundPlaylist = await _context.Playlists.FirstOrDefaultAsync(
                    p => p.YoutubeId == playlist.YoutubeId
                );

                if (foundPlaylist == null)
                {
                    throw new Exception(
                        $"Failed to update playlist with id '{playlist.YoutubeId}'. Playlist not found."
                    );
                }

                if (playlist.Title != null)
                    foundPlaylist.Title = playlist.Title;
                if (playlist.ETag != null)
                    foundPlaylist.ETag = playlist.ETag;
                if (playlist.Videos != null)
                    UpdateRelationships.UpdateCollection(foundPlaylist.Videos, playlist.Videos);

                serviceResponse.Data.Add(foundPlaylist);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<Playlist>>> DeletePlaylist(List<Playlist> playlists)
    {
        var serviceResponse = new ServiceResponse<List<Playlist>>();

        try
        {
            var playlistIds = playlists.Select(p => p.YoutubeId).ToArray();

            await _context.Playlists
                .Where(p => playlistIds.Contains(p.YoutubeId))
                .ExecuteDeleteAsync();
            serviceResponse.Data = playlists;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<string>>> PlaylistsNotFound(List<string> playlistIds)
    {
        var serviceResponse = new ServiceResponse<List<string>> { Data = new List<string>() };

        try
        {
            foreach (var playlistId in playlistIds)
            {
                var foundPlaylist = await _context.Playlists.FirstOrDefaultAsync(
                    p => p.YoutubeId == playlistId
                );

                if (foundPlaylist == null)
                {
                    serviceResponse.Data.Add(playlistId);
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
}
