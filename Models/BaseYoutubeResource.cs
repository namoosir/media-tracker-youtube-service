using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models;

public class BaseYoutubeResource : BaseEntity
{
    [Key]
    [GraphQLDescription("Unique identifier for the resource according to YouTube (External)")]
    public required string YoutubeId { get; set; }

    [GraphQLDescription("The title of the resource")]
    public required string Title { get; set; }

    public required string ETag { get; set; }
}
