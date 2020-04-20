using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace Test
{
    /// <summary>
    /// 读取设备信息测试
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(Devices));
            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(@"C:\Users\zhang\source\repos\Walkman Playlist Tools\Test\default-capability.xml", FileMode.Open);
            // Declare an object variable of the type to be deserialized.
            Devices test;
            /* Use the Deserialize method to restore the object's state with
            data from the XML document. */
            test = (Devices)serializer.Deserialize(fs);
            Console.WriteLine(test.Device[0].Identification.MarketName);
            Console.Read();
        }
    }
}
