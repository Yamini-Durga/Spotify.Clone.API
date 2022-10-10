using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class LikedSongDto
    {
        public int LikedSongId { get; set; }
        public int SongId { get; set; }
        public int UserId { get; set; }
    }
}
