using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ComponentBinding
    {
        [XmlAttribute("Field")]
        public string Field { get; set; }   
    }
    
    public class BindingElement : ComponentBinding
    {
        [XmlAttribute("Source")]
        public string Source { get; set; }
    }

    public class ComponentSetterElement : ComponentBinding
    {       
        [XmlAttribute("Value")]
        public string Value { get; set; }
    }
}