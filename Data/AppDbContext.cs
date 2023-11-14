using MediaTrackerYoutubeService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerYoutubeService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }

        //good idea to implement this when creating multiple models with complex relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship for liked videos
            modelBuilder.Entity<User>().HasMany(u => u.LikedVideos).WithMany(v => v.LikedByUsers);

            // Configure the relationship for disliked videos
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.DislikedVideos)
                .WithMany(v => v.DislikedByUsers);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is BaseEntity
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default
        )
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is BaseEntity
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.Entity is BaseEntity
                        && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
