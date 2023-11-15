using AutoMapper;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Data;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Dtos.User;

namespace MediaTrackerYoutubeService.Services.UserService;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public UserService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    private async Task<ServiceResponse<User>> AddUser(int userId)
    {
        var serviceResponse = new ServiceResponse<User>();

        try
        {
            var newUser = new User
            {
                UserId = userId,
                PlaylistsEtag = "",
                VideoPlaylists = new List<Playlist>(),
                DislikedVideosEtag = "",
                DislikedVideos = new List<Video>(),
                LikedVideosEtag = "",
                LikedVideos = new List<Video>(),
                SubscribedChannels = new List<Channel>()
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            serviceResponse.Data = newUser;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<User>> UpsertUser(int userId)
    {
        var serviceResponse = new ServiceResponse<User>();

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            if (user is null)
                return await AddUser(userId);

            serviceResponse.Data = user;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<User>> GetUser(int userId)
    {
        var serviceResponse = new ServiceResponse<User>();
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);
            if (user is null)
                throw new Exception($"No user with id {userId} exists");
            serviceResponse.Data = user;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<User>> UpdateUser(User user)
    {
        var serviceResponse = new ServiceResponse<User>();
        try
        {
            var found = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (found == null)
                throw new Exception("User doesn't exist");

            // found.SubscribedChannels.Clear();
            found.SubscribedChannels = user.SubscribedChannels;

            // found.VideoPlaylists.Clear();
            found.VideoPlaylists = user.VideoPlaylists;
            found.PlaylistsEtag = user.PlaylistsEtag;

            // found.LikedVideos.Clear();
            found.LikedVideos = user.LikedVideos;
            found.LikedVideosEtag = user.LikedVideosEtag;

            // found.DislikedVideos.Clear();
            found.DislikedVideos = user.DislikedVideos;
            found.DislikedVideosEtag = user.DislikedVideosEtag;

            await _context.SaveChangesAsync();

            serviceResponse.Data = found;
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }
}
