using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spotify.Clone.Data.Interfaces;
using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;

namespace Spotify.Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<AllUsersPayload>>> GetAllUsers()
        {
            ServiceResponse<AllUsersPayload> response = new ServiceResponse<AllUsersPayload>();
            response = await _userService.GetAllUsersAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ServiceResponse<UserPayload>>> GetUserById(int id)
        {
            ServiceResponse<UserPayload> response = new ServiceResponse<UserPayload>();
            response = await _userService.GetUserByIdAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<ServiceResponse<UpdateUserDto>>> UpdateUserById(int id, [FromBody] UpdateUserDto request)
        {
            ServiceResponse<UpdateUserDto> response = new ServiceResponse<UpdateUserDto>();
            response = await _userService.UpdateUserByIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteUserById(int id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _userService.DeleteUserByIdAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
