using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class GameObjectElement : RectTransformElement
    {
        [XmlAttribute]
        public string Path { get; set; }
        
        [XmlElement("Setter", typeof(SetterElement))]
        public List<SetterElement> Setters { get; set; }
    }

    public class SetterElement
    {
        [XmlAttribute]
        public string Path { get; set; }
        
        [XmlAttribute]
        public string Field { get; set; }
        
        [XmlAttribute]
        public string Component { get; set; }
        
        [XmlAttribute]
        public string Value { get; set; }
    }
    
    public class PrefabElement : RectTransformElement
    {
        [XmlAttribute]
        public string Prefab { get; set; }
        
        [XmlElement("Property", typeof(PrefabInstancePropertyElement))]
        public List<PrefabInstancePropertyElement> Properties { get; set; }
    }

    public class PrefabInstancePropertyElement
    {
        [XmlAttribute] public string Name { get; set; }
        [XmlAttribute] public string Value { get; set; }
    }
    
}