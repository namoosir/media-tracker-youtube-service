using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Services.DataSynchronizationService;
using Microsoft.AspNetCore.Mvc;

namespace MediaTrackerYoutubeService.Controllers;

[ApiController]
[Route("action/[controller]")]
public class SyncController : ControllerBase
{
    private readonly IDataSynchronizationService _dataSynchronizationService;

    public SyncController(IDataSynchronizationService dataSynchronizationService)
    {
        _dataSynchronizationService = dataSynchronizationService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<string>>> SyncAll(int userId)
    {
        var res = await _dataSynchronizationService.SyncData(userId);
        return Ok(res);
    }
}
