using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics;
using MediaTrackerYoutubeService.Schemas.YoutubeAPIResource;

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
        // public string LikedVideos() => BASE_URL + "/videos" + "?part=snippet" + "&myRating=like" + $"&key={_apiKey}";
        public string Playlists()  =>  BASE_URL + "/playlists" + "?part=snippet" + "&mine=true" + $"&key={_apiKey}";
        public string PlaylistItems(string playlistId) => BASE_URL + "/playlistItems" + "?part=snippet" + $"playlistId={playlistId}" +  $"&key={_apiKey}";
    }

    public async Task<PlaylistResponse> GetMyPlaylists()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.Playlists());

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            PlaylistResponse playlistResponse = JsonSerializer.Deserialize<PlaylistResponse>(responseContent);
            // Debug.WriteLine("PlaylistResponse: " + playlistResponse);
            return playlistResponse;
        }
        else throw new Exception("Failed to retrieve playlists. Status code: " + response.StatusCode);
    }

    public async Task<PlaylistItemResponse> GetMyPlaylistItems(string playlistId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.PlaylistItems(playlistId));

        if (response.IsSuccessStatusCode){
            string responseContent = await response.Content.ReadAsStringAsync();
            PlaylistItemResponse playlistItemResponse = JsonSerializer.Deserialize<PlaylistItemResponse>(responseContent);
            // Debug.WriteLine("PlaylistResponse: " + playlistItemResponse);
            return playlistItemResponse;
        }
        else throw new Exception("Failed to retrieve playlists. Status code: " + response.StatusCode);
    }

    public async Task<PlaylistItemResponse> GetLikedVideos()
    {
        return await GetMyPlaylistItems("LL");
    }
}