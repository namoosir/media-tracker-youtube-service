using AutoMapper;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Data;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Dtos.User;
using MediaTrackerYoutubeService.Services.Utils;

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
                SubscribedChannels = new List<Channel>(),
                SubscriptionsEtag = ""
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

    public async Task<ServiceResponse<User>> UpdateUser(UpdateUserDto updateUser)
    {
        var serviceResponse = new ServiceResponse<User>();
        try
        {
            var found = await _context.Users.FirstOrDefaultAsync(
                u => u.UserId == updateUser.UserId
            );
            if (found == null)
                throw new Exception("User doesn't exist");

            if (updateUser.PlaylistsEtag != null)
                found.PlaylistsEtag = updateUser.PlaylistsEtag;
            if (updateUser.LikedVideosEtag != null)
                found.LikedVideosEtag = updateUser.LikedVideosEtag;
            if (updateUser.DislikedVideosEtag != null)
                found.DislikedVideosEtag = updateUser.DislikedVideosEtag;
            if (updateUser.SubscriptionsEtag != null)
                found.SubscriptionsEtag = updateUser.SubscriptionsEtag;
            if (updateUser.UpdatedAt != null)
                found.UpdatedAt = (DateTime)updateUser.UpdatedAt;

            if (updateUser.LikedVideos != null)
                UpdateRelationships.UpdateCollection(found.LikedVideos, updateUser.LikedVideos);
            if (updateUser.DislikedVideos != null)
                UpdateRelationships.UpdateCollection(
                    found.DislikedVideos,
                    updateUser.DislikedVideos
                );
            if (updateUser.SubscribedChannels != null)
                UpdateRelationships.UpdateCollection(
                    found.SubscribedChannels,
                    updateUser.SubscribedChannels
                );
            if (updateUser.VideoPlaylists != null)
                UpdateRelationships.UpdateCollection(
                    found.VideoPlaylists,
                    updateUser.VideoPlaylists
                );

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
