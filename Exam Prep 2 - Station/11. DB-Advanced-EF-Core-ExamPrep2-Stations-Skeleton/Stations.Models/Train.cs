using Stations.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class Train
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]     // Unique, required
        public string TrainNumber { get; set; }

        public TrainType? Type { get; set; } // !!!!!!! YES 

        public ICollection<TrainSeat> TrainSeats { get; set; } =
            new HashSet<TrainSeat>();

        public ICollection<Trip> Trips { get; set; } =
            new HashSet<Trip>();
    }
}