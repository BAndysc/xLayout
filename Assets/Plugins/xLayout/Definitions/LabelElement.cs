using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class LabelElement : RectTransformElement
    {
        [XmlAttribute]
        public string Text { get; set; }
    
        [XmlAttribute]
        public string Align { get; set; }

        [XmlAttribute] 
        public string FontSize { get; set; } = "20";

        [XmlAttribute]
        public string VertAlign { get; set; }
    
        [XmlAttribute]
        public string Color { get; set; }
    
        [XmlAttribute]
        public string FitSize { get; set; }
    }
}