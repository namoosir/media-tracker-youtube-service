namespace MediaTrackerYoutubeService.Schemas;

public class UserInformation
{
    public required string Token { get; set; }
    public required int UserId { get; set; }

    public static UserInformation Build(string token, int userId)
    {
        var userInformation = new UserInformation { Token = token, UserId = userId };
        return userInformation;
    }
}
