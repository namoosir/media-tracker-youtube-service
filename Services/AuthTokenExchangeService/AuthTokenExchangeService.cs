using System.Linq.Expressions;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Schemas;
using Newtonsoft.Json;

namespace MediaTrackerYoutubeService.Services.AuthTokenExchangeService;

public class AuthTokenExchangeService : IAuthTokenExchangeService
{
    private readonly HttpClient _httpClient;

    private readonly IConfiguration _configuration;

    public AuthTokenExchangeService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<string>> YoutubeAuthTokenExchange(int userId)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            string baseUrl =
                _configuration["Endpoints:AuthService"]
                ?? throw new Exception("Missing AuthService Endpoint in Config");
            string url = $"{baseUrl}/PlatformConnection/{userId}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var platformConnection = JsonConvert.DeserializeObject<
                    ServiceResponse<PlatformConnection>
                >(responseContent);

                if (platformConnection is null)
                    throw new Exception("Failed to deserialize object.");

                serviceResponse.Data = platformConnection.Data!.AccessToken;
            }
            else
                throw new Exception(
                    $"Failed to fetch platform connection. Status code: " + response.StatusCode
                );
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }
}
