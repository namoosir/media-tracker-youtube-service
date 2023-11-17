using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Utils;

public static class ServiceResponseUtils
{
    public static T TryGetThrow<T>(ServiceResponse<T> response, string ErrorMessage = null)
    {
        if (!response.Success)
        {
            if (ErrorMessage == null)
                throw new Exception(response.Message);
            else
                throw new Exception(ErrorMessage);
        }
        else
            return response.Data;
    }
}
