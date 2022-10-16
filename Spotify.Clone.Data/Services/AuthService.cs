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

        public async Task<ServiceResponse<VerifyUserDto>> RegisterUser(UserRegistrationDto request)
        {
            ServiceResponse<VerifyUserDto> response = new ServiceResponse<VerifyUserDto>();
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

            response.Message = "Registration completed successfully.";
            response.Data = new VerifyUserDto
            {
                Email = user.Email,
                Token = user.VerificationToken
            };

            return response;
        }
        public async Task<ServiceResponse<UserResponseDto>> LoginUser(UserLoginDto request)
        {
            ServiceResponse<UserResponseDto> response = new ServiceResponse<UserResponseDto>();
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
                response.Message = "User not verified, Please verify.";
                return response;
            }
          
            string secretKey = _configuration.GetSection("AppSettings:Token").Value;
            string token = HelperMethods.CreateToken(user, secretKey);
            response.Message = "Logged in successfully.";
            response.Data = new UserResponseDto
            {
                Id = user.UserId,
                Token = token
            };

            return response;
        }
        public async Task<ServiceResponse<string>> VerifyUser(VerifyUserDto request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await GetUserByToken(request);
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
        public async Task<ServiceResponse<UserResponseDto>> ForgotPassword(ForgotPasswordDto request)
        {
            ServiceResponse<UserResponseDto> response = new ServiceResponse<UserResponseDto>();
            var user = await GetUser(request.Email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            user.ResetPasswordToken = HelperMethods.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(30);
            await _dbContext.SaveChangesAsync();
            response.Data = new UserResponseDto
            {
                Id = user.UserId,
                Token = user.ResetPasswordToken
        };
            response.Message = "Please, reset your password now.";

            return response; 
        }
        public async Task<ServiceResponse<UserResponseDto>> ResetPassword(ResetPasswordDto request)
        {
            ServiceResponse<UserResponseDto> response = new ServiceResponse<UserResponseDto>();
            var user = await GetUserByResetToken(request);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid token.";
                return response;
            }
            if(user != null && user.ResetTokenExpires < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Token expired!";
                return response;
            }
            HelperMethods.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetPasswordToken = null;
            user.ResetTokenExpires = null;
            await _dbContext.SaveChangesAsync();
            response.Message = "Passowrd changed successfully.";
            response.Data = new UserResponseDto
            {
                Id = user.UserId,
                Token = null
            };
            return response;
        }
        private async Task<bool> UserExists(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
        private async Task<User> GetUser(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
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
        private async Task<User> GetUserByToken(VerifyUserDto request)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.VerificationToken == request.Token && u.Email.ToLower() == request.Email.ToLower());
        }
        private async Task<User> GetUserByResetToken(ResetPasswordDto request)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ResetPasswordToken == request.Token && u.Email.ToLower() == request.Email.ToLower());
        }
    }
}
 