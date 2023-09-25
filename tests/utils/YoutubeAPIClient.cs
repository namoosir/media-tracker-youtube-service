using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using MediaTrackerYoutubeService.Utils.Youtube;
using MediaTrackerYoutubeService.Schemas;
    
[TestClass]
public class YoutubeAPIClientTests
{
    private const string BearerToken = "";
    private const string ApiKey = "";

    [TestMethod]
    public async Task GetLikedVideos_ValidRequest_ReturnsResponse()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        YoutubeAPIResponse response = await client.GetRatedVideos(YoutubeAPIClient.Rating.Like);

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
