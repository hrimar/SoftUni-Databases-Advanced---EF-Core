using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Import
{
    [XmlType("comment")]
    public class CommentDto
    {
      
       
            [Required]
            [XmlElement("content")]
            public string Content { get; set; }

            [Required]
            [StringLength(30)]
            [XmlElement("user")]
            public string User { get; set; } // Unique

            [Required]
            [XmlElement("post")]
            public CommentPostDto Post { get; set; }
       
    }
}
