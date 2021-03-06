﻿using System.ComponentModel.DataAnnotations;

namespace Instagraph.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Content { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }                
    }
}