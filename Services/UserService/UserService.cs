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

    private async Task<ServiceResponse<GetUserDto>> AddUser(int userId)
    {
        var serviceResponse = new ServiceResponse<GetUserDto>();

        try
        {
            var userDto = new GetUserDto { UserId = userId };
            var toInsert = _mapper.Map<User>(userDto);

            _context.Users.Add(toInsert);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetUserDto>(toInsert);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetUserDto>> UpsertUser(int userId)
    {
        var serviceResponse = new ServiceResponse<GetUserDto>();

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            if (user is null)
                return await AddUser(userId);

            serviceResponse.Data = _mapper.Map<GetUserDto>(user);
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
}
