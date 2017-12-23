using Stations.Models;
using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; } //required, not negativ!!!

        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{}$")] // st+6int !!!!!!!!!!!
       // [MaxLength(8)]     
        public string SeatingPlace { get; set; }

        public int TripId { get; set; } // required
        [Required]
        public Trip Trip { get; set; }

        public int? CustomerCardId { get; set; }   //    !!!!
        public CustomerCard CustomerCard { get; set; }
    }
}