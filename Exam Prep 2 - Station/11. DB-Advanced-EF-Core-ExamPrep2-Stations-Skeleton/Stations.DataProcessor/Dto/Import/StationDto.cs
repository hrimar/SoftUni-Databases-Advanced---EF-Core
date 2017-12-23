using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Stations.DataProcessor.Dto.Import
{
    public class StationDto
    {
        // гледаме какви данни идват от JSON стринга

        [Required]
        [MaxLength(50)]    
        public string Name { get; set; }

        //[Required]// но като се подават може да липсват zatova go mahame
        [MaxLength(50)]    
        public string Town { get; set; }
    }
}
