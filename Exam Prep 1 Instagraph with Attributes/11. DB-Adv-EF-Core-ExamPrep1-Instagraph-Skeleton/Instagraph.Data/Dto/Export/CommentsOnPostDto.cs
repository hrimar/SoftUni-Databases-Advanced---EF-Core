using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Export
{
    [XmlType("user")]
    public class CommentsOnPostDto
    {
        [XmlElement("Username")]
        public string Username { get; set; }

        [XmlElement("MostCommentse")]
        public int MostComments { get; set; }
    }
}
