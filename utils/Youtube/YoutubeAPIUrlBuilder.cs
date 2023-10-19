namespace MediaTrackerYoutubeService.Utils.Youtube;

public class YoutubeAPIUrlBuilder
{
    private const string BASE_URL = "https://www.googleapis.com/youtube/v3";

    public static string Build(string resource, IDictionary<string, string> queryParams, List<string>? part, List<string>? id)
    {
        var uriBuilder = new UriBuilder(BASE_URL);
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        var partString = "snippet";

        uriBuilder.Path += $"/{resource}";
        
        if (part is not null)
        {
            partString = string.Join(",", part);
        }

        query["part"] = partString;

        query["maxResults"] = "50";

        foreach (var key in queryParams.Keys)
        {
            if (queryParams[key] != null) query[key] = queryParams[key];
        }

        if (id is not null)
        {
            query["id"] = string.Join(",", id);
        }

        uriBuilder.Query = query.ToString();

        return uriBuilder.ToString();
    }
}