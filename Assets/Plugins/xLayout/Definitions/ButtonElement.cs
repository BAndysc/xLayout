using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ButtonElement : RectTransformElement
    {
        [XmlAttribute]
        public string Text { get; set; }
    }
}