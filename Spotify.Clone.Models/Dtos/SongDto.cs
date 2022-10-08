using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string SongUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double Duration { get; set; }
    }
}
