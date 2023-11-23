using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MediaTrackerYoutubeService.Models
{
    [GraphQLDescription("Model for YouTube channels")]
    public class Channel : BaseYoutubeResource
    {
        // [Key]
        // [GraphQLDescription("Unique identifier for the channel according to YouTube (External)")]
        // public required string YoutubeId { get; set; }

        // [GraphQLDescription("The title of the channel")]
        // public required string Title { get; set; }

        [GraphQLDescription("The videos created by this channel")]
        public virtual required ICollection<Video> Videos { get; set; }

        [GraphQLDescription("Subscribers to this Channel (Internal Users)")]
        public virtual required ICollection<User> UserSubscribers { get; set; }

        [GraphQLDescription("The number of subscribers for this channel")]
        public long? SubscriberCount { get; set; }

        [GraphQLDescription("The total number of views for this channel's videos")]
        public long? ViewCount { get; set; }

        [GraphQLDescription("The total number of videos uploaded to this channel")]
        public long? VideoCount { get; set; }

        [GraphQLDescription("URL of the channel's thumbnail image")]
        public string? ThumbnailUrl { get; set; }

        [GraphQLDescription("Categories the channel falls under (Cooking, Gaming, etc)")]
        [NotMapped]
        public ICollection<string> Categories
        {
            get { return JsonSerializer.Deserialize<ICollection<string>>(CategoriesJson); }
            set { CategoriesJson = JsonSerializer.Serialize(value); }
        }

        [Column("Categories")]
        public string CategoriesJson { get; set; }

        // public required string ETag { get; set; }

        public required bool Imported { get; set; }
    }
}
