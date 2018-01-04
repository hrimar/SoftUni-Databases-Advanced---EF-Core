﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Instagraph.Models
{
    public class Picture
    {
        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Size { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
