namespace MediaTrackerYoutubeService.Models.Utils;

// something give me int id; auth service send user id get back token => string accesstoken =>
public class UserInformation
{
    public required int UserId { get; set; }
    public required string Token { get; set; }
}
