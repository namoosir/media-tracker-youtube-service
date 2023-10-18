using AutoMapper;
using MediaTrackerYoutubeService.Models;
using MediaTrackerYoutubeService.Data;
using Microsoft.EntityFrameworkCore;
using MediaTrackerYoutubeService.Dtos.User;
using MediaTrackerYoutubeService.Models;
namespace MediaTrackerYoutubeService.Services.UserService;

public class UserService 
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public UserService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<ServiceResponse<UserIdDto>> AddUser(int newUserId)
    {
        var serviceResponse = new ServiceResponse<UserIdDto>();

        try
        {
            var userDto = new UserIdDto{ UserId = newUserId};
            var toInsert = _mapper.Map<User>(userDto);

            _context.Users.Add(toInsert);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<UserIdDto>(toInsert);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

//     public async Task<ServiceResponse<UserIdDto>> GetUserById(int userId)
//     {
//         var serviceResponse = new ServiceResponse<UserIdDto>();

//         try
//         {
//             var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

//             if (user is null)
//             {
//                 throw new Exception($"User with User Id '{userId}' not found");
//             }

//             serviceResponse.Data = _mapper.Map<UserIdDto>(user);
//         }
//         catch (Exception e)
//         {
//             serviceResponse.Success = false;
//             serviceResponse.Message = e.Message;
//         }

//         return serviceResponse;
//     }

//     public async Task<ServiceResponse<UserIdDto>> UpdateUser(UpdateUserDto updatedUser)
//     {
//         var serviceResponse = new ServiceResponse<UserIdDto>();

//         try
//         {
//             var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == updatedUser.UserId);

//             if (user is null)
//             {
//                 throw new Exception($"User with User Id '{updatedUser.UserId}' not found");
//             }

//             user.Platform = updatedUser.Platform;
//             user.PlatformId = updatedUser.PlatformId;

//             await _context.SaveChangesAsync();
//             serviceResponse.Data = _mapper.Map<UserIdDto>(user);
//         }
//         catch (Exception e)
//         {
//             serviceResponse.Success = false;
//             serviceResponse.Message = e.Message;
//         }
//         return serviceResponse;
//     }



public async Task<ServiceResponse<UserIdDto>> UpsertUser(int userId)
    {
        var serviceResponse = new ServiceResponse<UserIdDto>();

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            if (user is null) return await AddUser(userId);

            serviceResponse.Data = _mapper.Map<UserIdDto>(user);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }
}