using Spotify.Clone.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.Interfaces
{
    public interface ISongService
    {
        Task<ServiceResponse<SongDto>> CreateSongAsync(SongDto request);
        Task<ServiceResponse<IEnumerable<SongDto>>> GetAllSongsAsync();
        Task<ServiceResponse<SongDto>> UpdateSongAsync(int id, SongDto request);
        Task<ServiceResponse<string>> DeleteSongByIdAsync(int id);
        Task<ServiceResponse<string>> LikeSongByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<SongDto>>> GetAllLikedSongsAsync();
        Task<ServiceResponse<IEnumerable<SongDto>>> SearchAsync(string searchStr);
    }
}
