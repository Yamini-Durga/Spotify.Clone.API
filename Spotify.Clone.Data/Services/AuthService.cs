using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spotify.Clone.Data.DataContext;
using Spotify.Clone.Data.Interfaces;
using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly SpotifyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(SpotifyDbContext dbContext, IMapper mapper, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<object>> RegisterUser(UserRegistrationDto request)
        {
            ServiceResponse<object> response = new ServiceResponse<object>();
            if(await UserExists(request.Email))
            {
                response.Success = false;
                response.Message = "Email already exists, Please login.";
                return response;
            }
            HelperMethods.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = _mapper.Map<User>(request);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            user.VerificationToken = HelperMethods.CreateRandomToken();

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            response.Message = "Registerd Successfully.";
            response.Data = new
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Token = user.VerificationToken
            };

            return response;
        }
        public async Task<ServiceResponse<string>> LoginUser(UserLoginDto request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await GetUser(request.Email);
            if(user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            if (!HelperMethods.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
                return response;
            }
            if (user.VerifiedAt == null)
            {
                response.Success = false;
                response.Message = "User not verified.";
                return response;
            }
          
            string secretKey = _configuration.GetSection("AppSettings:Token").Value;
            string token = HelperMethods.CreateToken(user, secretKey);
            response.Message = "Logged in successfully.";
            response.Data = token;

            return response;
        }
        public async Task<ServiceResponse<string>> VerifyUser(string token)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await GetUserByToken(token);
            if(user == null)
            {
                response.Success = false;
                response.Message = "Invalid token";
                return response;
            }
            user.VerifiedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            response.Message = "Verified successfully.";

            return response;
        }
        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await GetUser(email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            user.ResetPasswordToken = HelperMethods.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(30);
            await _dbContext.SaveChangesAsync();
            response.Data = user.ResetPasswordToken;
            response.Message = "Please, reset your password now.";

            return response; 
        }
        public async Task<ServiceResponse<string>> ResetPassword(ResetPasswordDto request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await GetUserByResetToken(request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Invalid token.";
                return response;
            }
            HelperMethods.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetPasswordToken = null;
            user.ResetTokenExpires = null;
            await _dbContext.SaveChangesAsync();
            response.Message = "Passowrd changed successfully.";

            return response;
        }
        private async Task<bool> UserExists(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }
        private async Task<User> GetUser(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        private string GetUserRole()
        {
            string role = string.Empty;
            if(_httpContextAccessor.HttpContext != null)
            {
                role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            return role;
        }
        private async Task<User> GetUserByToken(string token)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
        }
        private async Task<User> GetUserByResetToken(string token)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }
    }
}
 