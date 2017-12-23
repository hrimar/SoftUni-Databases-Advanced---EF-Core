using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Stations.DataProcessor.Dto
{
    [XmlType("Card")]
    public class TicketCardDto
    {
        [Required]
        public string Name { get; set; }
    }
}
