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
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public string? VerificationToken { get; set; } // verified at registration time
        public DateTime? VerifiedAt { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; } // token expiration time for reset password
        [Required, RegularExpression("Male|Female")]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public string Month { get; set; } = string.Empty;
        [Required]
        public string Date { get; set; } = string.Empty;
        [Required]
        public string Year { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public ICollection<LikedSong>? LikedSongs { get; set; }
        public ICollection<Playlist>? Playlists { get; set; }
    }
}
