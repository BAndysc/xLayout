using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    [XmlRoot("Canvas", Namespace = "urn:xLayout")]
    public class CanvasDefinition : BaseElement
    {
        [XmlAttribute("schemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = "http://www.acme.com/xml/OrderXML-1-0.xsd";
        
        [XmlArray("Resources")]
        [XmlArrayItem("Variable", typeof(ResourceVariableElement))]
        [XmlArrayItem("Asset", typeof(ResourceAssetElement))]
        [XmlArrayItem("Include", typeof(ResourceIncludeElement))]
        [XmlArrayItem("Prefab", typeof(ResourcePrefabElement))]
        public List<ResourceElement> Resources { get; set; }
    }
    [XmlRoot("Resources", Namespace = "urn:xLayout")]
    public class ResourcesDefinition
    {
        [XmlAttribute("schemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = "http://www.acme.com/xml/OrderXML-1-0.xsd";
        
        [XmlElement("Variable", typeof(ResourceVariableElement))]
        [XmlElement("Asset", typeof(ResourceAssetElement))]
        [XmlElement("Include", typeof(ResourceIncludeElement))]
        [XmlElement("Prefab", typeof(ResourcePrefabElement))]
        public List<ResourceElement> Resources { get; set; }
    }
}