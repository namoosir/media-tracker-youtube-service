using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerYoutubeService.Services.StoreYoutubeDataService;

public class StoreYoutubeDataService : IStoreYoutubeDataService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public StoreYoutubeDataService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // public async Task<ServiceResponse<string>> StoreYoutubeData(List<UserVideo> userVideos)
    // {
    //     var serviceResponse = new ServiceResponse<string>();
    //     try
    //     {
    //         using (var dbContext = _contextFactory.CreateDbContext())
    //         {
    //             foreach (UserVideo userVideo in userVideos)
    //             {
    //                 dbContext.UserVideos.Add(userVideo);
    //             }
    //             await dbContext.SaveChangesAsync();
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         serviceResponse.Data = "Something went wrong";
    //         serviceResponse.Success = false;
    //         serviceResponse.Message = e.Message;
    //         return serviceResponse;
    //     }
    //     serviceResponse.Data = "Success!";
    //     return serviceResponse;
    // }
}
