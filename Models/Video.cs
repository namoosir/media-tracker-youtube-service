using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for individual YouTube videos")]
    public class Video : BaseEntity
    {
        [Key]
        [GraphQLDescription("Unique identifier for the video according to YouTube (External)")]
        public string YoutubeId { get; set; }

        [GraphQLDescription("The channel to which this video belongs")]
        public Channel Channel { get; set; }

        [GraphQLDescription("The number of views this video has received")]
        public int ViewCount { get; set; }

        [GraphQLDescription("The number of likes this video has received")]
        public int LikeCount { get; set; }

        [GraphQLDescription("The number of comments posted on this video")]
        public int CommentCount { get; set; }

        [GraphQLDescription("URL of the video's thumbnail image")]
        public string ThumbnailUrl { get; set; }
    }
}