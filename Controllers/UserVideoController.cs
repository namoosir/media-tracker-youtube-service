using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Models.Utils;
using MediaTrackerYoutubeService.Services.AuthTokenExchangeService;
using MediaTrackerYoutubeService.Services.FetchYoutubeDataService;
using MediaTrackerYoutubeService.Services.ProcessYoutubeDataService;
using MediaTrackerYoutubeService.Services.StoreYoutubeDataService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediaTrackerYoutubeService.Controllers;

[ApiController]
[Route("action/[controller]")]
public class UserVideoController : ControllerBase
{
    private readonly IAuthTokenExchangeService _authTokenExchangeService;
    private readonly IFetchYoutubeDataService _fetchYoutubeDataService;
    private readonly IProcessYoutubeDataService _processYoutubeDataService;
    private readonly IStoreYoutubeDataService _storeYoutubeDataService;

    public UserVideoController(
        IAuthTokenExchangeService authTokenExchangeService,
        IFetchYoutubeDataService fetchYoutubeDataService,
        IProcessYoutubeDataService processYoutubeDataService,
        IStoreYoutubeDataService storeYoutubeDataService
    )
    {
        _authTokenExchangeService = authTokenExchangeService;
        _fetchYoutubeDataService = fetchYoutubeDataService;
        _processYoutubeDataService = processYoutubeDataService;
        _storeYoutubeDataService = storeYoutubeDataService;
    }

    [HttpPost]
    public async Task<
        ActionResult<ServiceResponse<string>>
    > AuthenticateFetchProcessStoreYoutubeData(int userId)
    {
        //exchange service
        var userInformation = await _authTokenExchangeService.YoutubeAuthTokenExchange(userId);
        //stuff = fetch service
        var fetchedData = await _fetchYoutubeDataService.FetchYoutubeVideos(userInformation.Data);
        //process stuff
        var processedData = await _processYoutubeDataService.ProcessYoutubeData(fetchedData.Data);
        //save to db (stuff)
        var storeStatus = await _storeYoutubeDataService.StoreYoutubeData(processedData.Data);
        //return OK
        return Ok(storeStatus);

        //create the subscription thing maybe??
    }
}
