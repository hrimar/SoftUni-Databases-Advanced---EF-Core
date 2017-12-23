using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Stations.DataProcessor.Dto.Import
{
    [XmlType("Ticket")]
    public class TicketDto
    {
        [XmlAttribute("price")]
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; } //required, not negativ!!!

        [XmlAttribute("seat")]
        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{}$")] // st+6int !!!!!!!!!!!
        public string Seat { get; set; }

        [Required]
        public TicketTripDto Trip { get; set; }
               
        public TicketCardDto Card { get; set; }
    }

}
