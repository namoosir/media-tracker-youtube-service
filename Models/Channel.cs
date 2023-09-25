using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for YouTube channels")]
    public class Channel : BaseEntity
    {
        [Key]
        [GraphQLDescription("Unique identifier for the channel according to YouTube (External)")]
        public required string YoutubeId { get; set; }

        [GraphQLDescription("The title of the channel")]
        public required string Title { get; set; }

        [GraphQLDescription("The number of subscribers for this channel")]
        public int? SubscriberCount { get; set; }

        [GraphQLDescription("The total number of views for this channel's videos")]
        public int? ViewCount { get; set; }

        [GraphQLDescription("The total number of videos uploaded to this channel")]
        public int? VideoCount { get; set; }

        [GraphQLDescription("URL of the channel's thumbnail image")]
        public string? ThumbnailUrl { get; set; }

        public required string ETag { get; set; }
    }
}