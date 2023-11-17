using System.ComponentModel.DataAnnotations;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.User
{
    public class GetUserDto
    {
        public required int UserId { get; set; }
        public ICollection<Models.Channel>? SubscribedChannels { get; set; }
        public string? SubscriptionsEtag { get; set; }
        public ICollection<Models.Playlist>? VideoPlaylists { get; set; }

        public required string PlaylistsEtag { get; set; }
        public ICollection<Models.Video>? LikedVideos { get; set; }
        public required string LikedVideosEtag { get; set; }
        public ICollection<Models.Video>? DislikedVideos { get; set; }
        public required string DislikedVideosEtag { get; set; }
    }
}
