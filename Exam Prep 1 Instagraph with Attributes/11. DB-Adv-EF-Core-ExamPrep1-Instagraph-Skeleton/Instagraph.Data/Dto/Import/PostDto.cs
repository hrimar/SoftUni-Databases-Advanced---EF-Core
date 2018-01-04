using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Import
{
    [XmlType("post")]
    public class PostDto
    {
        [Required]
        [XmlElement("caption")]
        public string Caption { get; set; }

        [Required]
        [StringLength(30)]
        [XmlElement("user")]
        public string User { get; set; } // Unique

        [Required]
        [XmlElement("picture")]
        public string Picture { get; set; }
    }
}
