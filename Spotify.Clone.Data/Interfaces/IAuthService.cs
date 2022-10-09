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
        Task<ServiceResponse<object>> RegisterUser(UserRegistrationDto user);
        Task<ServiceResponse<string>> LoginUser(UserLoginDto user);
        Task<ServiceResponse<string>> VerifyUser(string token);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<ServiceResponse<string>> ResetPassword(ResetPasswordDto token);
    }
}
