using Newtonsoft.Json;

namespace MediaTrackerYoutubeService.Schemas;

public class YoutubeAPIResponse
{
    public required string kind { get; set; }
    public required string etag { get; set; }

    public string? nextPageToken { get; set; }

    public required PageInfo pageInfo { get; set; }

    public class PageInfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public required List<Resource> items { get; set; }
}

public class Resource
{
    public required string kind { get; set; }
    public required string etag { get; set; }
    public required string id { get; set; }

    public required Snippet snippet { get; set; }
    public Statistics? statistics { get; set; }

    public ContentDetails? contentDetails { get; set; }

    public TopicDetails? topicDetails { get; set; }
}

// Only for PlaylistItems Endpoint
public class ContentDetails
{
    public required string videoId { get; set; }

    public required string duration { get; set; }
}

public class TopicDetails
{
    public required List<string> topicCategories { get; set; }
}

public class Statistics
{
    public long? viewCount { get; set; }

    // Video Specific
    public long? likeCount { get; set; }
    public long? commentCount { get; set; }

    // Channels
    public long? subscriberCount { get; set; }
    public bool? hiddenSubscriberCount { get; set; }
    public long? videoCount { get; set; }
}

public class Snippet
{
    public required string title { get; set; }
    public required string description { get; set; }

    public string? channelId { get; set; }

    public string? channelTitle { get; set; }

    public string? videoOwnerChannelId { get; set; }

    public string? videoOwnerChannelTitle { get; set; }

    public int? categoryId { get; set; }

    public Thumbnails? thumbnails { get; set; }

    public class Thumbnails
    {
        [JsonProperty("default")]
        public Thumbnail? Default { get; set; }
        public Thumbnail? medium { get; set; }
    }

    public class Thumbnail
    {
        public required string url { get; set; }

        public override string ToString()
        {
            return $"URL: {url}";
        }
    }

    public ResourceId? resourceId { get; set; }

    // Specific only to Subsciptions, hence channelId
    public class ResourceId
    {
        public required string channelId { get; set; }
    }
}
