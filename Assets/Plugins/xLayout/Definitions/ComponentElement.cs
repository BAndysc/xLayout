using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ComponentElement
    {
        [XmlAttribute("Type")]
        public string ComponentType { get; set; }
    
        [XmlElement("Binding", typeof(BindingElement))]
        public List<BindingElement> Bindings { get; set; }
    }
}