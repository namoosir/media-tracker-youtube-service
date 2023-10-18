// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Moq;
// using MediaTrackerYoutubeService.Services.UserService;
// using MediaTrackerYoutubeService.Data;
// using MediaTrackerYoutubeService.Models;
// using MediaTrackerYoutubeService.Dtos.User;
// using System.Threading.Tasks;
// using System;
// using AutoMapper;

// [TestClass]
// public class UserServiceTests
// {
//     [TestMethod]
//     public async Task AddUser_ValidUserId_ReturnsServiceResponseWithUserId()
//     {
//         // Arrange
//         var userId = 123;
//         var userDto = new UserIdDto { UserId = userId };

//         var mockMapper = new Mock<IMapper>();
//         mockMapper.Setup(m => m.Map<User>(It.IsAny<UserIdDto>()))
//             .Returns(new User { UserId = userId });

//         var mockContext = new Mock<AppDbContext>();
//         mockContext.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

//         var userService = new UserService(mockMapper.Object, mockContext.Object);

//         // Act
//         var result = await userService.AddUser(userId);

//         // Assert
//         Assert.IsTrue(result.Success);
//         Assert.IsNotNull(result.Data);
//         Assert.AreEqual(userId, result.Data.UserId);
//     }

//     [TestMethod]
//     public async Task UpsertUser_UserExists_ReturnsServiceResponseWithUserId()
//     {
//         // Arrange
//         var userId = 123;
//         var existingUser = new User { UserId = userId };

//         var mockMapper = new Mock<IMapper>();
//         mockMapper.Setup(m => m.Map<UserIdDto>(It.IsAny<User>()))
//             .Returns(new UserIdDto { UserId = userId });

//         var mockContext = new Mock<AppDbContext>();
//         mockContext.Setup(c => c.Users.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
//             .ReturnsAsync(existingUser);

//         var userService = new UserService(mockMapper.Object, mockContext.Object);

//         // Act
//         var result = await userService.UpsertUser(userId);

//         // Assert
//         Assert.IsTrue(result.Success);
//         Assert.IsNotNull(result.Data);
//         Assert.AreEqual(userId, result.Data.UserId);
//     }

//     // Additional test cases can be added for edge cases, error handling, etc.
// }