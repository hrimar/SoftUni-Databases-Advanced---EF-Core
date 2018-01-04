using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Import
{
    [XmlType("post")]
    public class CommentPostDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}