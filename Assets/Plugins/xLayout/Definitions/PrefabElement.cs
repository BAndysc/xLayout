using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class PrefabElement : RectTransformElement
    {
        [XmlAttribute]
        public string Path { get; set; }
    }
}