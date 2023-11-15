using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Dtos.User;

namespace MediaTrackerYoutubeService.Services.UserService;

public interface IUserService
{
    Task<ServiceResponse<User>> GetUser(int userId);
    Task<ServiceResponse<User>> UpsertUser(int userId);

    Task<ServiceResponse<User>> UpdateUser(User user);
}
