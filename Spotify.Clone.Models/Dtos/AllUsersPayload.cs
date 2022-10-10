using Spotify.Clone.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class AllUsersPayload
    {
        public long TotalUsers { get; set; }
        public IEnumerable<UserPayload> Users { get; set; }
    }
}
