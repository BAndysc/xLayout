using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ComponentElement
    {
        [XmlAttribute("Type")]
        public string ComponentType { get; set; }
    
        [XmlElement("Binding", typeof(BindingElement))]
        [XmlElement("Setter", typeof(ComponentSetterElement))]
        public List<ComponentBinding> Bindings { get; set; }
    }
}