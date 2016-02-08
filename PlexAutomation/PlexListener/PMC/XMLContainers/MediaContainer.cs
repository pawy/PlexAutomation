using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlexListener.PMC.XMLContainers
{
    [XmlType("MediaContainer")]
    public class MediaContainer
    {
        [XmlElement("Timeline")]
        public List<Timeline> Timelines { get; set; }
    }
}