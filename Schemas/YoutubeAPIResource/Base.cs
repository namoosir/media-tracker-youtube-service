namespace MediaTrackerYoutubeService.Schemas.YoutubeAPIResource;
public class Resource
{
    public string kind { get; set; }
    public string etag { get; set; }

    public override string ToString(){return $"Kind: {kind}, Etag: {etag}";}
}

public class PageInfo
{
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }

    public override string ToString(){return $"Total Results: {totalResults}, Results Per Page: {resultsPerPage}";}
}

public class Snippet
{
    public string title { get; set; }
    public string description { get; set; }
    public Thumbnails? thumbnails { get; set; }

    public override string ToString(){return $"Title: {title}, Description: {description}, Thumbnails: {thumbnails}";}
}

public class Thumbnails
{
    public Thumbnail? Default { get; set; }
    public Thumbnail? Medium { get; set; }

    public override string ToString(){return $"Default: {Default}, Medium: {Medium}";}
}

public class Thumbnail
{
    public string url { get; set; }

    public override string ToString(){return $"URL: {url}";}
}