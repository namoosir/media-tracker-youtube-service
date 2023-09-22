
namespace MediaTrackerYoutubeService.Schemas.YoutubeAPIResource;
public class PlaylistItemResponse : Resource
{
    public PageInfo pageInfo { get; set; }
    public List<PlaylistItem> items { get; set; }

    public override string ToString(){
            string itemsString = items != null 
        ? string.Join(", ", items.Select(item => item.ToString()))
        : "NOITEMS";
        return $"{base.ToString()}, PageInfo: {pageInfo}, Items: {itemsString}";
    }
}

public class PlaylistItem : Resource
{
    public string Id { get; set; }

    public Snippet snippet { get; set; }

    public override string ToString(){return $"{base.ToString()}, Id: {Id}, Snippet: {snippet}";}
}





