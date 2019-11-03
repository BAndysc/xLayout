using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

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
        public string Offset { get; set; }
        
        [XmlAttribute]
        public string AnchorX { get; set; }
    
        [XmlAttribute]
        public string AnchorY { get; set; }

        [XmlAttribute]
        public string Pivot { get; set; }
    
        [XmlAttribute]
        public string Padding { get; set; }

        [XmlAttribute] public string Active { get; set; }
       
        [XmlAttribute] public string Top { get; set; }
    
        [XmlAttribute] public string Bottom { get; set; }
    
        [XmlAttribute] public string Left { get; set; }
    
        [XmlAttribute] public string Right { get; set; }
    
        [XmlAttribute]
        public string Dock { get; set; }

        [XmlArray("Components")]
        [XmlArrayItem("Component", typeof(ComponentElement))]
        public List<ComponentElement> Components { get; set; }
        
        [XmlArray("Animations")]
        [XmlArrayItem("CanvasAlphaAnimation", typeof(CanvasAlphaAnimationElement))]
        [XmlArrayItem("ScaleAnimation", typeof(ScaleAnimationElement))]
        [XmlArrayItem("PositionAnimation", typeof(PositionAnimationElement))]
        public List<AnimationElement> Animations { get; set; }
    }
}