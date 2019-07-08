using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class RectTransformElement : BaseElement
    {
        [XmlAttribute]
        public string Key { get; set; }
    
        [XmlAttribute]
        public string Width { get; set; }
    
        [XmlAttribute]
        public string Height { get; set; }
    
        [XmlAttribute("Name")]
        public string Name { get; set; }
    
        [XmlAttribute]
        public string AnchorX { get; set; }
    
        [XmlAttribute]
        public string AnchorY { get; set; }

        [XmlAttribute]
        public string Pivot { get; set; }
    
        [XmlAttribute]
        public string Padding { get; set; }

        [XmlAttribute] public string Active { get; set; }
       
        [XmlIgnore] public float? Top { get; set; }
    
        [XmlAttribute("Top")]
        public float TopBacking
        {
            get => Top ?? 0;
            set => Top = value;
        }
    
        [XmlIgnore] public float? Bottom { get; set; }
    
        [XmlAttribute("Bottom")]
        public float BottomBacking
        {
            get => Bottom ?? 0;
            set => Bottom = value;
        }
    
        [XmlIgnore] public float? Left { get; set; }
    
        [XmlAttribute("Left")]
        public float LeftBacking
        {
            get => Left ?? 0;
            set => Left = value;
        }
    
        [XmlIgnore] public float? Right { get; set; }
    
        [XmlAttribute("Right")]
        public float RightBacking
        {
            get => Right ?? 0;
            set => Right = value;
        }
    
        [XmlAttribute]
        public bool Dock
        {
            set
            {
                if (value)
                    AnchorX = AnchorY = "(0, 1)";
            }
            get { return false; }
        }

        [XmlArray("Components")]
        [XmlArrayItem("Component", typeof(ComponentElement))]
        public List<ComponentElement> Components { get; set; }
    }
}