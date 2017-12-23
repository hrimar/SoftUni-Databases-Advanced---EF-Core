using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Stations.DataProcessor.Dto.Import
{
    [XmlType("Card")]
    public class CardDto
    {
        [Required]
        [MaxLength(128)]     //  required
        public string Name { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public string CardType { get; set; } = "Normal";//
        
    }
}
