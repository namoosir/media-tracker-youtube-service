using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerYoutubeService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Platform> Platforms { get; set; }
    }
}
