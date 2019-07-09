using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ResourceVariableElement : ResourceElement
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
}