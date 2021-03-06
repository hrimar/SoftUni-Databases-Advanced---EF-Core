﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Item")]
    public class ItemsDto
    {
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement("Quantity")]
        [Range(1, int.MaxValue)] // !!!!!!!!!!!!!!!
        public int Quantity { get; set; }
    }
}
