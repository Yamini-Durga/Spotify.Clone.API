using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Date { get; set; }
        public int Year { get; set; }
        public int[] LikedSongs { get; set; } = new int[] { };
        public int[] Playlists { get; set; } = new int[] { };
    }
}
