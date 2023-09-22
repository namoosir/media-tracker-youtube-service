using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerYoutubeService.Services.UserVideoService;

public class UserVideoService : IUserVideoService
{
    private readonly IDbContextFactory<AppDbContext> _context;
    private readonly HttpClient _httpClient;

    public UserVideoService(IDbContextFactory<AppDbContext> context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public Task<ServiceResponse<string>> FetchAndStoreYoutubeDataByUserInformation(
        UserInformation userInformation
    )
    {
        throw new NotImplementedException();
    }
}
