using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class SongDto
    {
        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required, MinLength(2)]
        public string Artist { get; set; } = string.Empty;
        [Required]
        public string SongUrl { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public double Duration { get; set; }
    }
}
