using System.ComponentModel.DataAnnotations;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.Playlist;

public class UpdatePlaylistDto
{
    public required string YoutubeId { get; set; }
    public string? Title { get; set; }
    public ICollection<Models.Video>? Videos { get; set; }
    public Models.User? User { get; set; }
    public string? ETag { get; set; }
}
