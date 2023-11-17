using System.ComponentModel.DataAnnotations;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.User
{
    public class UpdateUserDto
    {
        public required int UserId { get; set; }
        public ICollection<Channel>? SubscribedChannels { get; set; }
        public ICollection<Models.Playlist>? VideoPlaylists { get; set; }
        public string? PlaylistsEtag { get; set; }
        public ICollection<Video>? LikedVideos { get; set; }
        public string? LikedVideosEtag { get; set; }
        public ICollection<Video>? DislikedVideos { get; set; }
        public string? DislikedVideosEtag { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
