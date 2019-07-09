using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ListViewElement  : RectTransformElement
    {
        [XmlAttribute]
        public string Items { get; set; }
    
        [XmlElement("ListView.Element")]
        public RectTransformElement ChildItem { get; set; }
    }
}