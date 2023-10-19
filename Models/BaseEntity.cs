namespace MediaTrackerYoutubeService.Models;

using HotChocolate;

[GraphQLDescription("Base Entity class for all other models to inherit from.")]
public class BaseEntity
{
    [GraphQLDescription("Created at this timestamp.")]
    public required DateTime CreatedAt { get; set; }

    [GraphQLDescription("Last updated at this timestamp.")]
    public required DateTime UpdatedAt { get; set; }

    // [GraphQLDescription("Youtube Hash.")]
    // public required string Etag { get; set; }
}
