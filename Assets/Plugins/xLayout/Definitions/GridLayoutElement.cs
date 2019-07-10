using System.Xml.Serialization;

namespace xLayout.Definitions
{
    public class GridLayoutElement : BaseElement
    {
        [XmlAttribute]
        public string Align { get; set; }
    
        [XmlAttribute]
        public string VertAlign { get; set; }
    
        [XmlAttribute]
        public string Padding { get; set; }
       
        [XmlAttribute]
        public string FitSize { get; set; }

        [XmlAttribute]
        public string Spacing { get; set; }
        
        [XmlAttribute]
        public string Axis{ get; set; }
        
        [XmlAttribute]
        public string CellSize { get; set; }
        
        [XmlAttribute]
        public string Columns { get; set; }
        
        [XmlAttribute]
        public string Rows { get; set; }
    }
}