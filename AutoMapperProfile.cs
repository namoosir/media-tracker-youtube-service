using AutoMapper;
using MediaTrackerYoutubeService.Dtos.User;
using MediaTrackerYoutubeService.Models;

namespace MediaTrackerYoutubeService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<GetUserDto, User>();
        }
    }
}
