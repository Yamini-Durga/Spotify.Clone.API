using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class PlaylistDto
    {
        [Required, Key]
        public int PlaylistId { get; set; }
        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required, MinLength(10)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public ICollection<SongDto>? Songs { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
