using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class ResourceAssetElement : ResourceElement
    {
        [XmlAttribute]
        public string Path { get; set; }
    }
}