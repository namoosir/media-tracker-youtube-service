using System.Text.Json.Serialization;

namespace MediaTrackerYoutubeService.Schemas
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MediaPlatform
    {
        Youtube = 1,
        Facebook = 2,
        Google = 3,
    }
}
