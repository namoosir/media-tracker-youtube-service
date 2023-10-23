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

        // public ... likedPlaylist
    }
}
