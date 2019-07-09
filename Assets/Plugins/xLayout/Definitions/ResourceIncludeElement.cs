using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ResourceIncludeElement : ResourceElement
    {
        [XmlAttribute]
        public string Path { get; set; }
    }
    
    
    public class ResourcePrefabElement : ResourceElement
    {
        [XmlElement]
        public BaseElement Content { get; set; }
        
        [XmlArray]
        [XmlArrayItem("Property", typeof(PropertyElement))]
        public List<PropertyElement> Properties { get; set; }
    }

    public class PropertyElement
    {
        [XmlAttribute] 
        public string Name { get; set; }
        
        [XmlIgnore]
        public bool HasDefault { get; set; }
        
        [XmlAttribute] 
        public bool Required { get; set; }

        [XmlIgnore]
        private string @default;
        
        [XmlAttribute]
        public string Default
        {
            get => @default;
            set
            {
                @default = value;
                HasDefault = true;
            }
        }
    }
}