using Microsoft.AspNetCore.Mvc;
using Spotify.Clone.Data.Interfaces;
using Spotify.Clone.Models.Dtos;

namespace Spotify.Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<object>>> Register(UserRegistrationDto request)
        {
            ServiceResponse<object> response = new ServiceResponse<object>();
            response = await _authService.RegisterUser(request);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _authService.LoginUser(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("verify")]
        public async Task<ActionResult<ServiceResponse<string>>> Verify(string token)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _authService.VerifyUser(token);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("forgotpassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _authService.ForgotPassword(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("resetpassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword(ResetPasswordDto token)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            response = await _authService.ResetPassword(token);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
