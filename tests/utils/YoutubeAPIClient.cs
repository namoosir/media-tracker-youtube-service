using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using MediaTrackerYoutubeService.Utils;
using MediaTrackerYoutubeService.Schemas.YoutubeAPIResource;
    
[TestClass]
public class YoutubeAPIClientTests
{
    private const string BearerToken = "ya29.a0AfB_byALVkjscHiIORp7JdS5IgD3jG8AqRN6HrGS-DnfgYTsJ-JKJADnGuV4lw4PMipy1nfrQJF4cEqTEP1AxmhNL2tetgOum8zAkpDm5Qb1t2mJVez2f2GqThyp2_n6paACTziT-MxiZzA6Cqhq2YsoT6Zh7aXRYOM5aCgYKAVkSARISFQGOcNnC8P4WvjnveTKiZWq6a-cvsw0171";
    private const string ApiKey = "AIzaSyBQTCRnQig9KQgzyMqpafkPGeBMKc-4i8g";

    [TestMethod]
    public async Task GetLikedVideos_ValidRequest_ReturnsResponse()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        PlaylistItemResponse response = await client.GetLikedVideos();

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GetMyPlaylists_ValidRequest_ReturnsResponse()
    {
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        PlaylistResponse response = await client.GetMyPlaylists();

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task istrue()
    {
        Assert.IsTrue(true);
    }
    
}