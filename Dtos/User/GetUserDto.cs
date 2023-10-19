using System.ComponentModel.DataAnnotations;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.User
{
    public class GetUserDto
    {
        public required int UserId { get; set; }
        public ICollection<Models.Channel>? SubscribedChannels { get; set; }
        public ICollection<Playlist>? VideoPlaylists { get; set; }
    }
}
