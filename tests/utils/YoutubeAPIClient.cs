using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using MediaTrackerYoutubeService.Utils.Youtube;
using MediaTrackerYoutubeService.Schemas;

namespace MediaTrackerYoutubeService.Tests.Utils;
[TestClass]
public class YoutubeAPIClientTests
{
    private const string BearerToken = "ya29.a0AfB_byD6aXKmdp1ucFvLdIwqo09nsTXs7aR-NM-PFe19no8qCE3NHQLTVM6b5cFq-Xraku9153fnS63_kGsROcu22VqV48YHtFN60lLvRp8OP0fSDpI99HsfeAxGXqpPTJvLI3zxLqFD-KhtPtXRuk9srMvB90pg2grTaCgYKAbcSARASFQGOcNnCCB-nQcRSg14VWhoaKXxaGg0171";
    private const string ApiKey = "AIzaSyCJ5a1wgdnM2RCSGybxP8bv-osZHziwNIA";

    [TestMethod]
    public async Task GetLikedVideos_ValidRequest_ReturnsResponse()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetRatedVideos(YoutubeAPIClient.Rating.Like);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetLikedVideosNextPage_ValidRequest_ReturnsResponse()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetRatedVideos(YoutubeAPIClient.Rating.Like, "CGQQAA");

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetMyPlaylists_ValidRequest_ReturnsResponse()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetMyPlaylists();

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetMyPlaylistsItems()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetMyPlaylistItems("PLBiSPTHLp980MPLjeaenyI_fo-VawsFwO");

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetMySubscriptions()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetSubscriptions();

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetChannels()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetChannels(new List<string>{"UCrTW8WZTlOZMvvn_pl1Lpsg","UCmDTrq0LNgPodDOFZiSbsww","UCRD2CerUvgKHQ"});

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetMyVideos()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetVideos(new List<string>{"i3AkTO9HLXo","coFIEH3vXPw","m_MQYyJpIjg"});

        Assert.IsNotNull(response);
    }    
}
