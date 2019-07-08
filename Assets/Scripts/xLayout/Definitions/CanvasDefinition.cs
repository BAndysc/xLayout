using System.Xml.Serialization;

namespace xLayout.Definitions
{
    [XmlRoot("Canvas", Namespace = "urn:xLayout")]
    public class CanvasDefinition : RectTransformElement
    {
        [XmlAttribute("schemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
        public string xsiSchemaLocation = "http://www.acme.com/xml/OrderXML-1-0.xsd";
    }
}