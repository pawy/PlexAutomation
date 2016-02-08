using System.Xml.Serialization;

namespace PlexListener.PMS.XMLContainers
{
    [XmlType("Video")]
    public class Video
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public User User { get; set; }
        public Player Player { get; set; }
    }
}
