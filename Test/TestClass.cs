using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{

    public class Inner
    {
        [XmlElement("inner-line-test")]
        public string InnerLine { get; set; }

        [XmlElement("inner-bool-test")]
        public bool InnerBool { get; set; }
    }

    public class InnerArray
    {
        [XmlElement("x-array")]
        public string[] Xarray { get; set; }
    }

    [XmlRoot("outer")]
    public class TestClass
    {
        [XmlElement("inner")]
        public Inner Inner { get; set; }
        [XmlElement("inner-array")]
        public InnerArray innerArray { get; set; }
    }
}
