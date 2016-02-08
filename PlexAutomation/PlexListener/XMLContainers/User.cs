using System.Xml.Serialization;

namespace PlexListener.XMLContainers
{
    [XmlType("User")]
    public class User
    {
        [XmlAttribute("title")]
        public string Title { get; set; }
    }
}
