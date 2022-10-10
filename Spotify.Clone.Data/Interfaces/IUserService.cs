using Spotify.Clone.Models.Dtos;
using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<AllUsersPayload>> GetAllUsersAsync();
        Task<ServiceResponse<UserPayload>> GetUserByIdAsync(int id);
        Task<ServiceResponse<UpdateUserDto>> UpdateUserByIdAsync(int id, UpdateUserDto request);
        Task<ServiceResponse<string>> DeleteUserByIdAsync(int id);
    }
}
