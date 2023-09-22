using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MediaTrackerYoutubeService.Utils;
public class YoutubeAPIClient
{
    
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly Endpoints _endpoints;

    public YoutubeAPIClient(string bearer_token, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer_token);
        _apiKey = apiKey;
        _endpoints = new Endpoints(_apiKey);
    }

    private class Endpoints
    {
        private readonly string _apiKey;    
        public Endpoints(string apiKey){_apiKey = apiKey;}

        const string BASE_URL = "https://www.googleapis.com/youtube/v3";
        public string LikedVideos => BASE_URL + "/videos" + "?part=snippet" + "&myRating=like" + "&key=" + _apiKey;
        public string Playlists => BASE_URL + "/playlists" + "?part=snippet" + "&mine=true" + "&key=" + _apiKey;
    }

    public async Task<string> GetLikedVideos()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.LikedVideos);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        else
        {
            throw new Exception("Failed to retrieve liked videos. Status code: " + response.StatusCode);
        }
    }

    public async Task<string> GetMyPlaylists()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.Playlists);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        else
        {
            throw new Exception("Failed to retrieve playlists. Status code: " + response.StatusCode);
        }
    }
}