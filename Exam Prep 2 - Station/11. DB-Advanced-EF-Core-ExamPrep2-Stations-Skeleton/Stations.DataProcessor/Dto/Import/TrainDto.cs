using Stations.Models;
using Stations.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Stations.DataProcessor.Dto.Import
{
    public class TrainDto
    {
        [Required]
        [MaxLength(30)]     // Unique, required
        public string TrainNumber { get; set; }

        public string Type { get; set; } = "HighSpeed";  // при null ще даде тази ст-ст
                                                         // тук от стринга идва стринг а не клас

        public SeatDto[] Seats { get; set; } = new SeatDto[0]; // тук поличаваме масив от др. ДТО!
    }

    
}
