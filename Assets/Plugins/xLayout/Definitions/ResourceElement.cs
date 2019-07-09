using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ResourceElement
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}