using System.Xml.Serialization;

namespace PlexListener.PMC.XMLContainers
{
    [XmlType("Timeline")]
    public class Timeline
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("state")]
        public string State { get; set; }
    }
}
