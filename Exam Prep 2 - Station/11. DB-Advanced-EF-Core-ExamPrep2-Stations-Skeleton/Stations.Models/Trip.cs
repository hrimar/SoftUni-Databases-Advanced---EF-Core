﻿using Stations.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Stations.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public int OriginStationId { get; set; } 
        [Required]
        public Station OriginStation { get; set; }

        public int DestinationStationId { get; set; }
        [Required]
        public Station DestinationStation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; } // required

        [Required]
        public DateTime ArrivalTime { get; set; } // required

        public int TrainId { get; set; } // required
        [Required]
        public Train Train { get; set; }

        public TripStatus Status { get; set; }

        public TimeSpan? TimeDifference { get; set; } // YES !!!
        
    }
}