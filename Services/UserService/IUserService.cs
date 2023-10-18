
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Dtos.User;
namespace MediaTrackerYoutubeService.Services.UserService;
public interface IUserService
{
    // Task<ServiceResponse<GetUserSubscriptionsDto>> GetUserSubscriptions(string userId);
    // Task<ServiceResponse<GetUserPlaylistsDto>> GetUserPlaylists(string userId);
    Task<ServiceResponse<UserIdDto>> UpsertUser(string userId);
}