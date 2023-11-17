namespace MediaTrackerYoutubeService.Utils.Auth;

public class AuthClient : IAuthClient
{
    public AuthClient() { }

    public async Task<string> GetAccessTokenByUserId(int userId)
    {
        throw new NotImplementedException();
    }
}
