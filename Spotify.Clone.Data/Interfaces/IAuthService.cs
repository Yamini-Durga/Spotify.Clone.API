using Spotify.Clone.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<VerifyUserDto>> RegisterUser(UserRegistrationDto user);
        Task<ServiceResponse<UserResponseDto>> LoginUser(UserLoginDto user);
        Task<ServiceResponse<string>> VerifyUser(VerifyUserDto request);
        Task<ServiceResponse<UserResponseDto>> ForgotPassword(ForgotPasswordDto email);
        Task<ServiceResponse<UserResponseDto>> ResetPassword(ResetPasswordDto token);
    }
}
