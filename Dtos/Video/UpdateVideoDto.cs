using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.Video;

public class UpdateVideoDto
{
    public required string YoutubeId { get; set; }
    public string? Title { get; set; }

    public Models.Channel? Channel { get; set; }

    public ICollection<Models.Playlist>? Playlist { get; set; }

    public long? ViewCount { get; set; }

    public long? LikeCount { get; set; }

    public long? CommentCount { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? ETag { get; set; }

    public bool? Imported { get; set; }

    public ICollection<Models.User>? LikedByUsers { get; set; }
    public ICollection<Models.User>? DislikedByUsers { get; set; }
}
