using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        public IQueryable<Platform> GetPlatform([ScopedService] AppDbContext context)
        {
            return context.Platforms;
        }
    }
}
