using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Stations.DataProcessor.Dto.Import
{
    public class SeatDto
    {
        [Required]
        [MaxLength(30)]     // Unique, required
        public string Name { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]     // да е точно 2 поз.
        public string Abbreviation { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Quantity { get; set; } // само за положителни int числа!
    }
}
