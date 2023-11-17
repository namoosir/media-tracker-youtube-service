using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService.Services.Utils;

public static class UpdateRelationships
{
    public static void UpdateCollection<T>(
        ICollection<T> existingCollection,
        ICollection<T> updatedCollection
    )
        where T : BaseYoutubeResource
    {
        // Update the collection by adding, updating, or removing items
        HashSet<string> existingItemsId = new(existingCollection.Select(x => x.YoutubeId));
        HashSet<string> updatedItemsId = new(updatedCollection.Select(x => x.YoutubeId));

        var onlyInExisiting = existingCollection.Where(x => !updatedItemsId.Contains(x.YoutubeId));
        var onlyInUpdated = updatedCollection.Where(x => !existingItemsId.Contains(x.YoutubeId));

        foreach (var item in onlyInExisiting)
        {
            existingCollection.Remove(item);
        }

        foreach (var item in onlyInUpdated)
        {
            existingCollection.Add(item);
        }
    }
}
