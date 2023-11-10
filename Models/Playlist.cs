using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for Youtube Video Playlists")]
    public class Playlist : BaseEntity
    {
        [Key]
        [GraphQLDescription("Unique identifier for playlist according to Youtube (External)")]
        public required string YoutubeId { get; set; }

        [GraphQLDescription("The title of the playlist")]
        public required string Title { get; set; }

        [GraphQLDescription("Videos comprising the playlist")]
        public virtual required ICollection<Video> Videos { get; set; }

        [GraphQLDescription("Owner of the playlist")]
        public virtual required User User { get; set; }

        public required string ETag { get; set; }
    }
}
