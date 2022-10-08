using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Models
{
    public class LikedSong
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int SongId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
