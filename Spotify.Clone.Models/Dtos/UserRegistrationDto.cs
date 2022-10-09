﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Clone.Models.Dtos
{
    public class UserRegistrationDto
    {
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
        public string Month { get; set; } = string.Empty;
        [Required]
        public string Date { get; set; } = string.Empty;
        [Required]
        public string Year { get; set; } = string.Empty;
    }
}
