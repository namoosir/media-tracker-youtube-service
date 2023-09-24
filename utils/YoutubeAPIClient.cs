using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Azure;

public class YoutubeAPIClient
{
    const string BASE_URL = "https://www.googleapis.com/youtube/v3";
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public YoutubeAPIClient(string bearer_token, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            bearer_token
        );
        _apiKey = apiKey;
    }

    public async Task<string> GetLikedVideos()
    {
        var uriBuilder = new UriBuilder(BASE_URL);
        var query = HttpUtility.ParseQueryString(string.Empty);

        uriBuilder.Path += "/videos";
        List<string> part = new() { "snippet", "contentDetails", "statistics" };

        // Convert the list to a comma-separated string
        string partString = string.Join(",", part);

        query["part"] = partString;
        query["myRating"] = "like";
        query["key"] = _apiKey;

        uriBuilder.Query = query.ToString();

        //perform query and then return the created object for another service to process and insert
        var endpoint = uriBuilder.ToString();
        var res = await _httpClient.GetAsync(endpoint);

        if (res.IsSuccessStatusCode)
        {
            var responseContentString = await res.Content.ReadAsStringAsync();
            return responseContentString;
        }
        else
        {
            throw new Exception($"Error getting liked videos: {res.StatusCode}");
        }
    }

    public async Task<string> GetDislikedVideos()
    {
        var uriBuilder = new UriBuilder(BASE_URL);
        var query = HttpUtility.ParseQueryString(string.Empty);

        uriBuilder.Path += "/videos";
        List<string> part = new() { "snippet", "contentDetails", "statistics" };

        string partString = string.Join(",", part);

        query["part"] = partString;
        query["myRating"] = "dislike";
        query["key"] = _apiKey;

        uriBuilder.Query = query.ToString();

        var endpoint = uriBuilder.ToString();
        var res = await _httpClient.GetAsync(endpoint);

        if (res.IsSuccessStatusCode)
        {
            var responseContentString = await res.Content.ReadAsStringAsync();
            return responseContentString;
        }
        else
        {
            throw new Exception($"Error getting disliked videos: {res.StatusCode}");
        }
    }

    public async Task<string> GetUserSubscriptions()
    {
        string endpoint = BASE_URL + "/videos" + "?part=snippet" + "&myRating=like";
    }

    public async Task<string> GetUserPlaylists()
    {
        string endpoint = BASE_URL + "/videos" + "?part=snippet" + "&myRating=like";
    }
}
