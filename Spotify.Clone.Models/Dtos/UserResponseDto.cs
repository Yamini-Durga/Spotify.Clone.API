using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
