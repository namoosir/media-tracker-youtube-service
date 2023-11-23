using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.Channel;

public class UpdateChannelDto
{
    public required string YoutubeId { get; set; }

    public string? Title { get; set; }

    public ICollection<Models.Video>? Videos { get; set; }

    public ICollection<Models.User>? UserSubscribers { get; set; }

    public ICollection<string>? Categories { get; set; }

    public long? SubscriberCount { get; set; }

    public long? ViewCount { get; set; }

    public long? VideoCount { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? ETag { get; set; }

    public bool? Imported { get; set; }
}
