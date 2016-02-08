
using System.Xml.Serialization;

namespace PlexListener.XMLContainers
{
    [XmlType("Player")]
    public class Player
    {
        [XmlAttribute("address")]
        public string Address { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("state")]
        public string State { get; set; }
    }
}
