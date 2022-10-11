using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Spotify.Clone.Data.DataContext;
using Spotify.Clone.Data.Interfaces;
using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.Services
{
    public class UserService : IUserService
    {
        private readonly SpotifyDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(SpotifyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AllUsersPayload>> GetAllUsersAsync()
        {
            ServiceResponse<AllUsersPayload> response = new ServiceResponse<AllUsersPayload>();
            try
            {
                var users = await _dbContext.Users
                    .Include(l => l.LikedSongs)
                    .ToListAsync();
                
                var usersPayload = _mapper.Map<IEnumerable<UserPayload>>(users);

                response.Message = "All users";
                response.Data = new AllUsersPayload
                {
                    TotalUsers = usersPayload.Count(),
                    Users = usersPayload
                };

                return response;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<UserPayload>> GetUserByIdAsync(int id)
        {
            ServiceResponse<UserPayload> response = new ServiceResponse<UserPayload>();
            try
            {
                var user = await _dbContext.Users
                    .Include(l => l.LikedSongs)
                    .SingleOrDefaultAsync(u => u.UserId == id);
                if(user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }
                var userPayload = _mapper.Map<UserPayload>(user);
                response.Message = "Single user";
                response.Data = userPayload;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<UpdateUserDto>> UpdateUserByIdAsync(int id, UpdateUserDto request)
        {
            ServiceResponse<UpdateUserDto> response = new ServiceResponse<UpdateUserDto>();
            try
            {
                var user = await _dbContext.Users
                    .SingleOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }
                if(user.Email != request.Email)
                {
                    var userExists = await CheckUserExists(request.Email, request.Id);
                    if (userExists == null)
                    {
                        user.Email = request.Email;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "User with email already exists with different id.";
                        return response;
                    }
                }
                user.UserId = user.UserId;
                user.Name = request.Name;
                user.Gender = request.Gender;
                user.Month = request.Month;
                user.Date = request.Date;
                user.Year = request.Year;
                await _dbContext.SaveChangesAsync();

                var userPayload = _mapper.Map<UpdateUserDto>(user);
                userPayload.Id = user.UserId;
                response.Message = "Updated user successfully";
                response.Data = userPayload;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<ServiceResponse<string>> DeleteUserByIdAsync(int id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();

                response.Message = "Deleted user successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        private async Task<User> CheckUserExists(string email, int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.UserId != id);
        }
    }
}
