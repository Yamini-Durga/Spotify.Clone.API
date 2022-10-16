using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class UserRegistrationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
    }
}
