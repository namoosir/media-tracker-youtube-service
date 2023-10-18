using System.ComponentModel.DataAnnotations;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Dtos.User
{
    public class UserIdDto
    {
        public required int  UserId   { get; set; }
    }
}
