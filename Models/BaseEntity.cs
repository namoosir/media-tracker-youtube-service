namespace MediaTrackerYoutubeService.Models;
using HotChocolate;

[GraphQLDescription("Base Entity class for all other models to inherit from.")]
public class BaseEntity
{
    [GraphQLDescription("Created at this timestamp.")]
    public DateTime CreatedAt { get; set; }

    [GraphQLDescription("Last updated at this timestamp.")]
    public DateTime UpdatedAt { get; set; }
}