namespace MediaTrackerYoutubeService.Utils.Auth;

public interface IAuthClient
{
    Task<string> GetAccessTokenByUserId(int userId);
}
