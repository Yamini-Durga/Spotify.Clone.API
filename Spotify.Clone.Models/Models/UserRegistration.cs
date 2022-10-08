using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Models
{
    public class UserRegistration
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required, MinLength(6), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required, RegularExpression("Male|Female")]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public int Month { get; set; }
        [Required]
        public int Date { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
