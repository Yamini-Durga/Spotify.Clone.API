﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Models
{
    public class Playlist
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required, MinLength(10)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public ICollection<PlaylistSong>? PlaylistSongs { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
