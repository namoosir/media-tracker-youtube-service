using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;



public class YoutubeAPIClient
{
    const string BASE_URL = "https://www.googleapis.com/youtube/v3";
    private readonly HttpClient _httpClient;
    private readonly string apiKey;

    public YoutubeAPIClient(string bearer_token, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer_token);
        _apiKey = apiKey;
    }

    public async Task<string> GetLikedVideos(string endpoint)
    {
        string endpoint = BASE_URL + "/videos" + "?part=snippet" + "&myRating=like"

    }

}