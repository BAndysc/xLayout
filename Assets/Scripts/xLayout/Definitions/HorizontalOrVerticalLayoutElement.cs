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
        public bool ExpandWidth { get; set; }
    
        [XmlAttribute]
        public bool ExpandHeight { get; set; }
    
        [XmlAttribute]
        public bool FitSize { get; set; }

        [XmlAttribute]
        public float Spacing { get; set; }
    
        [XmlIgnore]
        public bool? Flex { get; set; }

        [XmlAttribute("Flex")]
        public bool Flex_
        {
            get => Flex ?? false;
            set => Flex = value;
        }
    }
}