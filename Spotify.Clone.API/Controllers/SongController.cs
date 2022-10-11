using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Clone.Data.Interfaces;
using Spotify.Clone.Models.Dtos;

namespace Spotify.Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<SongDto>>> CreateSong(SongDto request)
        {
            ServiceResponse<SongDto> response = new ServiceResponse<SongDto>();
            response = await _songService.CreateSongAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SongDto>>>> GetAllSongs()
        {
            ServiceResponse<IEnumerable<SongDto>> response = new ServiceResponse<IEnumerable<SongDto>>();
            response = await _songService.GetAllSongsAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<SongDto>>> UpdateSong(int id, [FromBody]SongDto request)
        {
            ServiceResponse<SongDto> response = new ServiceResponse<SongDto>();
            response = await _songService.UpdateSongAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteSong(int id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _songService.DeleteSongByIdAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("like/{id}"), Authorize]
        public async Task<ActionResult<ServiceResponse<string>>> LikeSong(int id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _songService.LikeSongByIdAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("like"), Authorize]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SongDto>>>> GetAllLikedSongs()
        {
            ServiceResponse<IEnumerable<SongDto>> response = new ServiceResponse<IEnumerable<SongDto>>();
            response = await _songService.GetAllLikedSongsAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("search/{searchStr}"), Authorize]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SongDto>>>> Search(string searchStr)
        {
            ServiceResponse<IEnumerable<SongDto>> response = new ServiceResponse<IEnumerable<SongDto>>();
            response = await _songService.SearchAsync(searchStr);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
