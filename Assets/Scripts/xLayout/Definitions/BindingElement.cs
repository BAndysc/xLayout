using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class BindingElement
    {
        [XmlAttribute("Field")]
        public string Field { get; set; }
        [XmlAttribute("Source")]
        public string Source { get; set; }

    }
}