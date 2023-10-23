using System.Net.Http;
using System.Threading.Tasks;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTrackerYoutubeService.Tests.Services
{
    [TestClass]
    public class AuthTokenExchangeServiceTests
    {
        [TestMethod]
        public async Task YoutubeAuthTokenExchange_ValidResponse_ReturnsAccessToken()
        {
            // Arrange
            int userId = 8;
            string expectedAccessToken =
                "ya29.a0AfB_byBNHgwUi_NawZ078bv-tvdpZ-4Oaftv8wKnyCcxScQesnxIepMTkml0tJzQbixPzTDxNzSQMuWt4JvidvN0SBK2OnlICEYMHnauE0UyXmKN5Q4LOb_Rzm-wMlSLGl24DmXSuUN7xVfmLdvtbiZB0W2KS0qBYnISaCgYKAdMSARASFQGOcNnCE7VZIho8k8k2BlCAVL-Yjw0171";

            var client = new HttpClient();

            // Use the actual URL where your AuthServer is running
            string authServiceBaseUrl = "http://localhost:5238";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            configuration["Endpoints:AuthService"] = authServiceBaseUrl;

            var authTokenExchangeService = new AuthTokenExchangeService(client, configuration);

            // Act
            var result = await authTokenExchangeService.YoutubeAuthTokenExchange(userId);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedAccessToken, result.Data);
        }
    }
}
