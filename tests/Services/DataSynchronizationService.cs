// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Moq;
// using System.Threading.Tasks;
// namespace MediaTrackerYoutubeService.Tests.Services
// {
//     [TestClass]
//     public class DataSynchronizationServiceTests
//     {
//         [TestMethod]
//         public async Task SyncData_ValidUserId_CallsUserServiceUpsertUser()
//         {
//             // Arrange
//             int userId = 1;
//             var userServiceMock = new Mock<IUserService>();
//             userServiceMock.Setup(x => x.UpsertUser(userId)).ReturnsAsync(new OperationResult { Success = true });

//             var dataSyncService = new DataSynchronizationService(
//                 context: null,
//                 authTokenExchangeService: null,
//                 userService: userServiceMock.Object,
//                 configuration: null,
//                 fetchYoutubeDataService: null,
//                 processYoutubeDataService: null,
//                 storeYoutubeDataService: null
//             );

//             // Act
//             await dataSyncService.SyncData(userId);

//             // Assert
//             userServiceMock.Verify(x => x.UpsertUser(userId), Times.Once);
//         }

//         // Add more tests as needed for different scenarios and edge cases.
//     }
// }
