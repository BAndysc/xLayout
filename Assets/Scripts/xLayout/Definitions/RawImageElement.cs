using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class RawImageElement : RectTransformElement
    {
        [XmlAttribute]
        public string Color { get; set; }
    
        [XmlAttribute]
        public string Image { get; set; }
        
        [XmlAttribute]
        public string Material { get; set; }
    }
}