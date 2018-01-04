using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Instagraph.Data.Dto.Import
{
    public class UserFollowerDto
    {
        [Required]
        [StringLength(30)]
        public string User { get; set; } // Unique

        [Required]
        [StringLength(30)]
        public string Follower { get; set; } // Unique
    }
}
