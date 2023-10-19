using System.ComponentModel.DataAnnotations;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Schemas
{
    public class PlatformConnection
    {
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string Scopes { get; set; }
    }
}
