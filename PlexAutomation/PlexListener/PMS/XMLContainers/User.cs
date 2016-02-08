using System.Xml.Serialization;

namespace PlexListener.PMS.XMLContainers
{
    [XmlType("User")]
    public class User
    {
        [XmlAttribute("title")]
        public string Title { get; set; }
    }
}
