using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Instagraph.Data.Dto.Import
{
   public class PictureDto
    {
        [Required]
        public string Path { get; set; }

        [Required]
        public decimal Size { get; set; }
    }
}
