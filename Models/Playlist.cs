using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for Youtube Video Playlists")]
    public class Playlist : BaseEntity
    {
        [Key]
        [GraphQLDescription("Unique identifier for playlist according to Youtube (External)")]
        public string YoutubeId { get; set; }

        [GraphQLDescription("Unique identifier for user who created playlist (Internal)")]
        public string UserId { get; set; }

        [GraphQLDescription("Videos comprising the playlist")]
        public ICollection<Video> Videos { get; set; }
        

    }
}
