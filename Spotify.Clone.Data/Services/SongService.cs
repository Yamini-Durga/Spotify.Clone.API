using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Spotify.Clone.Data.DataContext;
using Spotify.Clone.Data.Interfaces;
using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.Services
{
    public class SongService : ISongService
    {
        private readonly SpotifyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SongService(SpotifyDbContext dbContext, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<SongDto>> CreateSongAsync(SongDto request)
        {
            ServiceResponse<SongDto> response = new ServiceResponse<SongDto>();
            try
            {
                if (await SongExists(request))
                {
                    response.Success = false;
                    response.Message = "Song already exists.";
                    return response;
                }
                Song song = _mapper.Map<Song>(request);
                await _dbContext.Songs.AddAsync(song);
                await _dbContext.SaveChangesAsync();

                response.Message = "Added song successfully.";
                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<SongDto>>> GetAllSongsAsync()
        {
            ServiceResponse<IEnumerable<SongDto>> response = new ServiceResponse<IEnumerable<SongDto>>();
            try
            {
                var songs = await _dbContext.Songs.ToListAsync();

                response.Message = "All songs";
                response.Data = _mapper.Map<IEnumerable<SongDto>>(songs);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<SongDto>> UpdateSongAsync(int id, SongDto request)
        {
            ServiceResponse<SongDto> response = new ServiceResponse<SongDto>();
            try
            {
                var song = await _dbContext.Songs.FirstOrDefaultAsync(s => s.SongId == id);
                if(song == null)
                {
                    response.Success = false;
                    response.Message = "Song not found.";
                    return response;
                }
                song.Name = request.Name;
                song.Artist = request.Artist;
                song.SongUrl = request.SongUrl;
                song.ImageUrl = request.ImageUrl;
                song.Duration = request.Duration;
                song.AddedAt = DateTime.Now;
                song.ImageUrlName = request.ImageUrlName;
                song.SongUrlName = request.SongUrlName;
                await _dbContext.SaveChangesAsync();

                response.Message = "Updated successfully.";
                response.Data = _mapper.Map<SongDto>(song);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<ServiceResponse<string>> DeleteSongByIdAsync(int id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                var song = await _dbContext.Songs.FirstOrDefaultAsync(s => s.SongId == id);
                if(song == null)
                {
                    response.Success = false;
                    response.Message = "Song not found.";
                    return response;
                }
                _dbContext.Songs.Remove(song);
                await _dbContext.SaveChangesAsync();
                response.Message = "Deleted song successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<ServiceResponse<string>> LikeSongByIdAsync(int id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                var song = await _dbContext.Songs.FirstOrDefaultAsync(s => s.SongId == id);
                if (song == null)
                {
                    response.Success = false;
                    response.Message = "Song not found.";
                    return response;
                }
                var curUserId = GetCurrentAuthUser();
                var likedSong = await CheckLikedSongs(curUserId, id);
                if (likedSong == null)
                {
                    LikedSong lSong = new LikedSong
                    {
                        SongId = id,
                        UserId = curUserId
                    };
                    await _dbContext.LikedSongs.AddAsync(lSong);
                    await _dbContext.SaveChangesAsync();

                    response.Message = "Added to wishlist";
                }
                else
                {
                    LikedSong lSong = new LikedSong
                    {
                        LikedSongId = likedSong.LikedSongId,
                        SongId = id,
                        UserId = curUserId
                    };
                    _dbContext.LikedSongs.Remove(lSong);
                    await _dbContext.SaveChangesAsync();

                    response.Message = "Removed from wishlist";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<SongDto>>> GetAllLikedSongsAsync()
        {
            ServiceResponse<IEnumerable<SongDto>> response = new ServiceResponse<IEnumerable<SongDto>>();
            try
            {
                int curUserId = GetCurrentAuthUser();
                response.Message = "All liked songs.";
                response.Data = await LikedSongsByUserId(curUserId);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<ServiceResponse<IEnumerable<SongDto>>> SearchAsync(string searchStr)
        {
            ServiceResponse<IEnumerable<SongDto>> response = new ServiceResponse<IEnumerable<SongDto>>();
            try
            {
                var songs = await _dbContext.Songs.Where(s => s.Name.ToLower().Contains(searchStr.ToLower())).ToListAsync();

                response.Message = "All liked songs.";
                response.Data = _mapper.Map<IEnumerable<SongDto>>(songs);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        private async Task<bool> SongExists(SongDto request)
        {
            var song = await _dbContext.Songs
                .FirstOrDefaultAsync(s => s.Name.ToLower() == request.Name.ToLower() &&
                    s.Artist.ToLower() == request.Artist.ToLower());
            return song != null;
        }
        private int GetCurrentAuthUser()
        {
            int userId = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                var id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                userId = Convert.ToInt32(id);
            }
            return userId;
        }
        private async Task<User> GetCurrentUserById(int id)
        {
            return  await _dbContext.Users
                    .Include(l => l.LikedSongs)
                    .SingleOrDefaultAsync(u => u.UserId == id);
        }
        private async Task<LikedSong> CheckLikedSongs(int userid, int songid)
        {
            return await _dbContext.LikedSongs.AsNoTracking()
                .SingleOrDefaultAsync(l => l.SongId == songid && l.UserId == userid);
        }
        private async Task<IEnumerable<SongDto>> LikedSongsByUserId(int userid)
        {
            string sqlQuery = $"SELECT S.* FROM Songs AS S JOIN LikedSongs AS LS " +
                $"ON S.SongId = LS.SongId WHERE LS.UserId = {userid};";
            var songs = await _dbContext.Songs.FromSqlRaw(sqlQuery).ToListAsync();

            return _mapper.Map<IEnumerable<SongDto>>(songs);
        }
    }
}
