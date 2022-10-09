using AutoMapper;
using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;

namespace Spotify.Clone.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegistrationDto, User>().ReverseMap();
        }
    }
}
