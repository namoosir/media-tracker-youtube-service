using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for individual YouTube videos")]
    public class Video : BaseYoutubeResource
    {
        // [Key]
        // [GraphQLDescription("Unique identifier for the video according to YouTube (External)")]
        // public required string YoutubeId { get; set; }

        // [GraphQLDescription("The title of the video")]
        // public required string Title { get; set; }

        [GraphQLDescription("The channel to which this video belongs")]
        public virtual required Channel Channel { get; set; }

        [GraphQLDescription("Playlists that has this video")]
        public virtual required ICollection<Playlist> Playlist { get; set; }

        [GraphQLDescription("The number of views this video has received")]
        public int? ViewCount { get; set; }

        [GraphQLDescription("The number of likes this video has received")]
        public int? LikeCount { get; set; }

        [GraphQLDescription("The number of comments posted on this video")]
        public int? CommentCount { get; set; }

        [GraphQLDescription("URL of the video's thumbnail image")]
        public required string ThumbnailUrl { get; set; }

        // public required string ETag { get; set; }

        public required bool Imported { get; set; }

        public virtual required ICollection<User> LikedByUsers { get; set; }
        public virtual required ICollection<User> DislikedByUsers { get; set; }
    }
}
