using HotChocolate.Resolvers;
using MediaTrackerYoutubeService.Controllers;
using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;


namespace MediaTrackerYoutubeService.GraphQL
{
    // public class Query
    // {
    //     [UseDbContext(typeof(AppDbContext))]
    //     [UsePaging]
    //     [UseProjection]
    //     [UseFiltering]
    //     [UseSorting]
    //     public IQueryable<UserVideo> GetUserVideo([ScopedService] AppDbContext context, int userId)
    //     {
    //         Console.WriteLine("THIS IS THE FUCKING ID LIL BRO " + userId);
    // public class Query
    // {
    //     [UseDbContext(typeof(AppDbContext))]
    //     [UsePaging]
    //     [UseProjection]
    //     [UseFiltering]
    //     [UseSorting]
    //     public IQueryable<UserVideo> GetUserVideo(
    //         [ScopedService] AppDbContext context,
    //         UserInformation user
    //     )
    //     {
    //         Console.WriteLine("THIS IS THE FUCKING ID LIL BRO " + user.UserId);

    //         // UserVideoController.FetchAndStoreYoutubeData();
    //         return context.UserVideos;
    //     }
    // }
}
