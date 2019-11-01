using System.Collections.Generic;
using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class AnimationElement
    {
        [XmlArray("Triggers")]
        [XmlArrayItem("PointerEnterEvent", typeof(OnPointerEnterTriggerElement))]
        [XmlArrayItem("PointerExitEvent", typeof(OnPointerExitTriggerElement))]
        [XmlArrayItem("PointerDownEvent", typeof(OnPointerDownTriggerElement))]
        [XmlArrayItem("PointerUpEvent", typeof(OnPointerUpTriggerElement))]
        [XmlArrayItem("EnableEvent", typeof(OnEnableTriggerElement))]
        public List<TriggerElement> Triggers { get; set; }
    }
    
    public class CanvasAlphaAnimationElement : AnimationElement
    {
        [XmlAttribute]
        public string DestValue { get; set; }
    }
    
    public class ScaleAnimationElement : AnimationElement
    {
        [XmlAttribute]
        public string DestValue { get; set; }
        
        [XmlAttribute]
        public string Speed { get; set; }
    }

    public class ConditionsContainer
    {
        [XmlArray("Conditions")]
        [XmlArrayItem("PointerIsOver", typeof(ConditionPointerOverElement))]
        [XmlArrayItem("Not", typeof(ConditionNotElement))]
        public List<ConditionElement> Conditions { get; set; }        
    }
    
    public class TriggerElement : ConditionsContainer
    {
    }
    
    public class OnPointerEnterTriggerElement : TriggerElement {}
    public class OnPointerExitTriggerElement : TriggerElement {}
    public class OnPointerDownTriggerElement : TriggerElement {}
    public class OnPointerUpTriggerElement : TriggerElement {}
    public class OnEnableTriggerElement : TriggerElement {}

    public class ConditionElement
    {
    }
    
    public class ConditionPointerOverElement : ConditionElement {}

    public class ConditionNotElement : ConditionElement
    {
        [XmlElement("PointerIsOver", typeof(ConditionPointerOverElement))]
        [XmlElement("Not", typeof(ConditionNotElement))]
        public List<ConditionElement> Conditions { get; set; }
    }
}