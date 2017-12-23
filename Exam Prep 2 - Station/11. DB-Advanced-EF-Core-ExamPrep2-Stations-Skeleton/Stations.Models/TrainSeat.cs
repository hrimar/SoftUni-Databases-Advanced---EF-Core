using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class TrainSeat
    {
        public int Id { get; set; }

        public int TrainId { get; set; }
        [Required]
        public Train Train { get; set; }

        public int SeatingClassId { get; set; }
        [Required]
        public SeatingClass SeatingClass { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } // само за положителни int числа!

        // пример:
        //[Range(typeof(decimal), "0", "79228162514264337593543950335")]
        //public decimal MyProperty { get; set; }
    }
}