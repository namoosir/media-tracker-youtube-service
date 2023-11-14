using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for users")]
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [GraphQLDescription("Unique identifier for user (Internal)")]
        public required int UserId { get; set; }

        [GraphQLDescription("Channels that the user is subscribed to")]
        public virtual required ICollection<Channel> SubscribedChannels { get; set; }

        [GraphQLDescription("Video playlists created by the user")]
        public virtual required ICollection<Playlist> VideoPlaylists { get; set; }

        [GraphQLDescription("Video playlists created by the user (Etag)")]
        public required string PlaylistsEtag { get; set; }

        [GraphQLDescription("Liked User playlist")]
        public virtual required ICollection<Video> LikedVideos { get; set; }

        [GraphQLDescription("Liked User playlist (Etag)")]
        public required string LikedVideosEtag { get; set; }

        [GraphQLDescription("Disliked User playlist")]
        public virtual required ICollection<Video> DislikedVideos { get; set; }

        [GraphQLDescription("Disliked User playlist (Etag)")]
        public required string DislikedVideosEtag { get; set; }
    }
}
