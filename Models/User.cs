using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for users")]
    public class User : BaseEntity
    {
        [Key]
        [GraphQLDescription("Unique identifier for user (Internal)")]
        public required string UserId { get; set; }

        [GraphQLDescription("Channels that the user is subscribed to")]
        public required ICollection<Channel> SubscribedChannels { get; set; }

        [GraphQLDescription("Video playlists created by the user")]
        public required ICollection<Playlist> VideoPlaylists { get; set; }
    }
}