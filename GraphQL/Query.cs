using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<UserVideo> GetUserVideo([ScopedService] AppDbContext context)
        {
            return context.UserVideos;
        }
    }
}
