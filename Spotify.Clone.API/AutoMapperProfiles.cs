using AutoMapper;
using Newtonsoft.Json;
using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;

namespace Spotify.Clone.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegistrationDto, User>().ReverseMap();
            CreateMap<User, UserPayload>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<LikedSong, LikedSongDto>().ReverseMap();
            CreateMap<Playlist, PlaylistDto>().ReverseMap();
            CreateMap<Song, SongDto>().ReverseMap();
        }
    }
}
