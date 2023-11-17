using HotChocolate.Resolvers;
using MediaTrackerYoutubeService.Controllers;
using MediaTrackerYoutubeService.Data;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUser([ScopedService] AppDbContext context)
        {
            return context.Users;
        }
    }
}
