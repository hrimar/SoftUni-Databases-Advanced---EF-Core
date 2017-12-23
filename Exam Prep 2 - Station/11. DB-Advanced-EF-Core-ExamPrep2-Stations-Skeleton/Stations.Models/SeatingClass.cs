using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class SeatingClass
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]     // Unique, required
        public string Name { get; set; }

        [StringLength(2, MinimumLength = 2)]     // да е точно 2 поз.
        public string Abbreviation { get; set; }
    }
}