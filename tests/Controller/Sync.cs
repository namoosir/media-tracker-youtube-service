using MediaTrackerYoutubeService.Controllers;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Services.DataSynchronizationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MediaTrackerYoutubeService.Tests.Controller;

[TestClass]
public class SyncControllerTests
{
    [TestMethod]
    public async Task SyncAll_Success()
    {
        // Arrange
        var userId = 2012; // Replace with a valid user ID in your system

        var serviceProvider = TestStartup.ConfigureServices();
        var scope = serviceProvider.CreateScope();
        var dataSynchronizationService =
            scope.ServiceProvider.GetRequiredService<IDataSynchronizationService>();

        var syncController = new SyncController(dataSynchronizationService);

        // Act
        var result = await syncController.SyncAll(userId) as ActionResult<ServiceResponse<string>>;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Value.Success);
        Assert.AreEqual("Success", result.Value.Data);
    }

    // [TestCleanup]
    // public void TestCleanup()
    // {
    //     // Dispose of services or perform cleanup as needed
    //     _serviceProvider?.Dispose();
    // }


    // Add more test methods for error cases, edge cases, etc.
}
