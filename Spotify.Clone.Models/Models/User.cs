using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        [Required, RegularExpression("Male|Female")]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public int Month { get; set; }
        [Required]
        public int Date { get; set; }
        [Required]
        public int Year { get; set; }
        public string Role { get; set; } = string.Empty;
        public ICollection<LikedSong>? LikedSongs { get; set; }
        public ICollection<Playlist>? Playlists { get; set; }
    }
}
