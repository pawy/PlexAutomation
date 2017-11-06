using System.Xml.Serialization;

namespace DenonListener.XMLContainers
{
    [XmlType("InputFuncSelectMain")]
    public class InputFuncSelectMain
    {
        [XmlElement("value")]
        public string Value { get; set; }
    }
}
