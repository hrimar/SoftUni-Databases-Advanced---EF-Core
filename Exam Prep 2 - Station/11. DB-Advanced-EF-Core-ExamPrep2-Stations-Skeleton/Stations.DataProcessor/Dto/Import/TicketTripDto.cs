using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Stations.DataProcessor.Dto.Import
{
    [XmlType("Trip")]
    public class TicketTripDto
    {
        [Required]
        public string OriginStation { get; set; }
        [Required]
        public string DestinationStation { get; set; }
        [Required]
        public string DepartureTime { get; set; }
    }
}
