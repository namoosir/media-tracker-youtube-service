using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using MediaTrackerYoutubeService.Utils;
    
[TestClass]
public class YoutubeAPIClientTests
{
    private const string BearerToken = "";
    private const string ApiKey = "A";

    [TestMethod]
    public async Task GetLikedVideos_ValidRequest_ReturnsResponse()
    {
        // Arrange
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        // Act
        string response = await client.GetLikedVideos();

        // Assert
        Assert.IsNotNull(response);
        // Add more specific assertions based on the expected response content or status codes
    }

    [TestMethod]
    public async Task GetMyPlaylists_ValidRequest_ReturnsResponse()
    {
        // Arrange
        YoutubeAPIClient client = new YoutubeAPIClient(BearerToken, ApiKey);

        // Act
        string response = await client.GetMyPlaylists();

        // Assert
        Assert.IsNotNull(response);
        // Add more specific assertions based on the expected response content or status codes
    }
}