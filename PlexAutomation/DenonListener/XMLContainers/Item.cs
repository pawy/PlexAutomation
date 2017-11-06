using System.Collections.Generic;
using System.Xml.Serialization;

namespace DenonListener.XMLContainers
{
    [XmlType("item")]
    public class Item
    {
        [XmlElement("InputFuncSelectMain")]
        public InputFuncSelectMain InputFuncSelectMain { get; set; }
    }
}