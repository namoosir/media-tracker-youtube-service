// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using System;
// using System.Threading.Tasks;
// using MediaTrackerYoutubeService.Services.UserService;
// using MediaTrackerYoutubeService.Dtos.User;
// using MediaTrackerYoutubeService.Models;
// using MediaTrackerYoutubeService.Data;
// using MediaTrackerYoutubeService.Services;
// using AutoMapper;

// namespace MediaTrackerYoutubeService.Tests.Services
// {
//     [TestClass]
//     public class UserServiceTests
//     {
//         private AppDbContext _context;

//         public UserServiceTests()
//         {
//             // Set up your DbContext here
//             var options = new DbContextOptionsBuilder<AppDbContext>()
//                 .UseLazyLoadingProxies()
//                 .UseSqlServer(
//                     "Server=localhost,1434;Database=YoutubeDB;User Id=sa;Password=pa55w0rd!;TrustServerCertificate=True"
//                 )
//                 .Options;

//             _context = new AppDbContext(options);
//         }

//         [TestMethod]
//         public async Task GetUser_ValidUserId_ReturnsUser()
//         {
//             // // Arrange
//             // var options = new DbContextOptionsBuilder<AppDbContext>()
//             //     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             //     .Options;

//             // // Set up some sample data
//             // using (var context = new AppDbContext(options))
//             // {
//             var channels = new List<Channel>();
//             var playlists = new List<Playlist>();
//             var videos = new List<Video>();

//             var user = new User{
//                 UserId = 1,
//                 CreatedAt = DateTime.Now,
//                 UpdatedAt = DateTime.Now,
//                 SubscribedChannels = channels,
//                 VideoPlaylists = playlists
//             };

//             var c1 = new Channel
//             {
//                 UserSubscribers = new List<User>(),
//                 Videos = videos,
//                 CreatedAt = DateTime.Now,
//                 UpdatedAt = DateTime.Now,
//                 YoutubeId = "stringsdgg",
//                 Title = "broodsfm",
//                 ETag = "adsffsdfggsdf"
//             };

//             var c2 = new Channel
//             {
//                 UserSubscribers = new List<User>(),
//                 Videos = videos,
//                 CreatedAt = DateTime.Now,
//                 UpdatedAt = DateTime.Now,
//                 YoutubeId = "strasaing",
//                 Title = "broom",
//                 ETag = "adsfasdf"

//             };

//             var c3 = new Channel
//             {
//                 UserSubscribers = new List<User>(),
//                 Videos = videos,
//                 CreatedAt = DateTime.Now,
//                 UpdatedAt = DateTime.Now,
//                 YoutubeId = "223g4",
//                 Title = "234asdf",
//                 ETag = "zxcvzcvz"
//             };


//             var v1 =  new Video
//             {
//                 Playlist = playlists,
//                 CreatedAt = DateTime.Now,
//                 UpdatedAt = DateTime.Now,
//                 ETag = "sdf",
//                 Channel = c1,
//                 ThumbnailUrl = "sda",
//                 YoutubeId = "sxfsd"
//             };

//             var p1 = new Playlist
//             {
//                 User = user,
//                 CreatedAt = DateTime.Now,
//                 UpdatedAt = DateTime.Now,
//                 ETag = "sdf",
//                 Videos = videos,
//                 YoutubeId = "fdsfdfsfdsf"
//             };

//             playlists.Add(p1);
//             videos.Add(v1);
//             channels.Add(c1);
//             channels.Add(c2);
//             channels.Add(c3);

//             // _context.Users.Add(
//             //     new User
//             //     {
//             //         UserId = 1,
//             //         CreatedAt = DateTime.Now,
//             //         UpdatedAt = DateTime.Now,
//             //         SubscribedChannels = channels,
//             //         VideoPlaylists = playlists
//             //     }
//             // );
//             // _context.SaveChanges();
//             // }

//             var mockMapper = new Mock<IMapper>();
//             // var mockContext = new AppDbContext(options);

//             var userService = new UserService(mockMapper.Object, _context);

//             // Act
//             var result = await userService.GetUser(1);

//             var play = result.Data.VideoPlaylists;
//             var subv = result.Data.SubscribedChannels;
//             // Assert
//             Assert.IsTrue(result.Success);
//             Assert.IsNotNull(result.Data);
//             Assert.AreEqual(1, result.Data.UserId);

//             // Assert.AreEqual(result.Data.SubscribedChannels[0], channels[0])
//             // Assert.AreEqual(1, result.Data.UserId);
//         }

//         [TestMethod]
//         public async Task GetUser_InvalidUserId_ReturnsNull()
//         {
//             // Arrange
//             var options = new DbContextOptionsBuilder<AppDbContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             var mockMapper = new Mock<IMapper>();

//             var userService = new UserService(mockMapper.Object, _context);

//             // Act
//             var result = await userService.GetUser(999); // Assuming there's no user with UserId 999

//             // Assert
//             Assert.IsFalse(result.Success);
//             Assert.IsNull(result.Data);
//             Assert.IsNotNull(result.Message); // You might want to assert the specific error message here
//         }
//     }
// }
