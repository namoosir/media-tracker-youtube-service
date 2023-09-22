
namespace MediaTrackerYoutubeService.Schemas.YoutubeAPIResource;
public class PlaylistResponse : Resource
{
    public string? nextPageToken { get; set; }
    public PageInfo pageInfo { get; set; }
    public List<Playlist> items { get; set; }

    public override string ToString(){
            string itemsString = items != null 
        ? string.Join(", ", items.Select(item => item.ToString()))
        : "NOITEMS";
        return $"{base.ToString()}, Next Page Token: {nextPageToken}, PageInfo: {pageInfo}, Items: {itemsString}";
    }
}

public class Playlist : Resource
{
    public string Id { get; set; }

    public Snippet snippet { get; set; }

    public override string ToString(){return $"{base.ToString()}, Id: {Id}, Snippet: {snippet}";}
}





