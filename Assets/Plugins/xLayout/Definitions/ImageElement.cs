using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ImageElement : RectTransformElement
    {
        [XmlAttribute]
        public string Color { get; set; }
    
        [XmlAttribute]
        public string Image { get; set; }
    
        [XmlAttribute]
        public string PreserveAspect { get; set; }
        
        [XmlAttribute]
        public string Material { get; set; }
    }
}