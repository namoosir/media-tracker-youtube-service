using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics;
using MediaTrackerYoutubeService.Schemas;
using System.Web;
using Azure;
using Newtonsoft.Json;

namespace MediaTrackerYoutubeService.Utils.Youtube;

public class YoutubeAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly Endpoints _endpoints;
    public YoutubeAPIClient(string bearer_token, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            bearer_token
        );
        _apiKey = apiKey;
        _endpoints = new Endpoints(_apiKey);
    }

    public enum Rating{
        Like,
        Dislike
    }


    private class Endpoints
    {
        private readonly string _apiKey;
        public Endpoints(string apiKey) => _apiKey = apiKey;

        public string RatedVideos(string rating) => YoutubeAPIUrlBuilder.Build(
                                                        YoutubeResource.Videos, 
                                                        new Dictionary<string, string> {{"myRating", $"{rating}"}, {"key", _apiKey}},
                                                        new List<string> {"snippet", "statistics"},
                                                        null);

        public string Playlists() => YoutubeAPIUrlBuilder.Build(
                                            YoutubeResource.Playlists,
                                            new Dictionary<string, string> {{"mine", "true"}, {"key", _apiKey}},
                                            null,
                                            null);

        public string PlaylistItems(string playlistId) => YoutubeAPIUrlBuilder.Build(
                                                                YoutubeResource.PlaylistItems,
                                                                new Dictionary<string, string> {{"playlistId", playlistId}, {"key", _apiKey}},
                                                                new List<string> {"snippet", "contentDetails"},
                                                                null);

        public string Subscriptions() => YoutubeAPIUrlBuilder.Build(
                                                                YoutubeResource.Subscriptions, 
                                                                new Dictionary<string, string> {{"mine", "true"}, {"key", _apiKey}}, 
                                                                null,
                                                                null);

        public string Channels(List<string> channelIds) => YoutubeAPIUrlBuilder.Build(
                                                            YoutubeResource.Channels, 
                                                            new Dictionary<string, string> {
                                                            {"key", _apiKey}}, 
                                                            new List<string> {"snippet", "statistics"}, 
                                                            channelIds);
        

        public string Videos(List<string> videoIds) => YoutubeAPIUrlBuilder.Build(
                                                        YoutubeResource.Videos, 
                                                        new Dictionary<string, string> {{"key", _apiKey}},
                                                        new List<string> {"snippet", "statistics"},
                                                        videoIds);
    }

    public async Task<YoutubeAPIResponse> GetMyPlaylists()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.Playlists());
        return await DeserializedResponse(response, YoutubeResource.Playlists);
    }

    public async Task<YoutubeAPIResponse> GetMyPlaylistItems(string playlistId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.PlaylistItems(playlistId));
        return await DeserializedResponse(response, YoutubeResource.PlaylistItems);
    }


    public async Task<YoutubeAPIResponse> GetRatedVideos(Rating rating)
    {
        string strRating = (rating == Rating.Like) ? "like" : "dislike";
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.RatedVideos(strRating));
        return await DeserializedResponse(response, YoutubeResource.Videos);
    }

    public async Task<YoutubeAPIResponse> GetSubscriptions()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.Subscriptions());
        return await DeserializedResponse(response, YoutubeResource.Videos);
    }

    public async Task<YoutubeAPIResponse> GetVideos(List<string> videoIds)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.Videos(videoIds));
        return await DeserializedResponse(response, YoutubeResource.Videos);
    }

    public async Task<YoutubeAPIResponse> GetChannels(List<string> channelIds)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_endpoints.Channels(channelIds));
        return await DeserializedResponse(response, YoutubeResource.Videos);
    }

    private async Task<YoutubeAPIResponse> DeserializedResponse(HttpResponseMessage response, string resource){
        if (response.IsSuccessStatusCode){
            string responseContent = await response.Content.ReadAsStringAsync();
            YoutubeAPIResponse? playlistItemResponse = JsonConvert.DeserializeObject<YoutubeAPIResponse>(responseContent);
            // Debug.WriteLine("PlaylistResponse: " + playlistItemResponse);
            return playlistItemResponse ?? throw new Exception("Failed to deserialize object.");
        }
        else throw new Exception($"Failed to retrieve {resource}. Status code: " + response.StatusCode);
    }
}