using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Stations.DataProcessor.Dto.Import
{
    public class TripDto
    {
        [Required]
        public string Train { get; set; } // required
        
        [Required]
        public string OriginStation { get; set; }

        [Required]
        public string DestinationStation { get; set; }

        [Required]
        public string DepartureTime { get; set; } // required

        [Required]
        public string ArrivalTime { get; set; } // required

        public string Status { get; set; } = "OnTime";

        public string TimeDifference { get; set; } 
    }
}
