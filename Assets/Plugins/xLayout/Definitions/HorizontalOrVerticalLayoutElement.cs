using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class HorizontalOrVerticalLayoutElement : BaseElement
    {
        [XmlAttribute]
        public string Align { get; set; }
    
        [XmlAttribute]
        public string VertAlign { get; set; }
    
        [XmlAttribute]
        public string Padding { get; set; }
    
        [XmlAttribute]
        public string ExpandWidth { get; set; }
    
        [XmlAttribute]
        public string ExpandHeight { get; set; }
    
        [XmlAttribute]
        public string FitSize { get; set; }

        [XmlAttribute]
        public string Spacing { get; set; }
    
        [XmlAttribute]
        public string Flex { get; set; }
    }
}