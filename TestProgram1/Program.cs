using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram1
{
    class Program
    {
        static void Main(string[] args)
        {
            string a=Path.ChangeExtension(@"C:\Users\zhang\Desktop\a.txt", "ppt");
            Console.WriteLine(a);
        }
    }
}
