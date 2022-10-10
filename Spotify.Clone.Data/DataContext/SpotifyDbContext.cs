using Microsoft.EntityFrameworkCore;
using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Data.DataContext
{
    public class SpotifyDbContext : DbContext
    {
        public SpotifyDbContext(DbContextOptions<SpotifyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<LikedSong> LikedSongs { get; set; }
    }
}
