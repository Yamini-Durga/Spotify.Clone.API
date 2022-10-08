using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Models
{
    public class PlaylistSong
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int SongId { get; set; }
        [Required]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
