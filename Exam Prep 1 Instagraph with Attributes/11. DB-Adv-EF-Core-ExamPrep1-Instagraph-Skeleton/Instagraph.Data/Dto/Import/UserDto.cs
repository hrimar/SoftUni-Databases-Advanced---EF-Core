﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Instagraph.Data.Dto.Import
{
    public class UserDto
    {
        [Required]
        [StringLength(30)]
        public string Username { get; set; } // Unique

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        public string ProfilePicture { get; set; }
    }
}
