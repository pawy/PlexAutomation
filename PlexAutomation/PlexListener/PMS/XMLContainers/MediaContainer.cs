using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlexListener.PMS.XMLContainers
{
    [XmlType("MediaContainer")]
    public class MediaContainer
    {
        [XmlElement("Video")]
        public List<Video> Videos { get; set; }

        [XmlElement("Photo")]
        public List<Photo> Photos { get; set; }
    }
}