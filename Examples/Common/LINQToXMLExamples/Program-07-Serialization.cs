using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void SerializeWithSaveOptions()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = XElement.Parse(@"<Root> <Child> Text </Child> </Root>");

            // Save XML without formatting.
            xmlTree.Save("root.xml", SaveOptions.DisableFormatting);
            Console.WriteLine(File.ReadAllText("root.xml"));

            Console.WriteLine("---");

            // Save XML with formatting.
            xmlTree.Save("root.xml", SaveOptions.None);
            Console.WriteLine(File.ReadAllText("root.xml"));
        }

        static void SerializeWithXmlDeclaration()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = XElement.Parse(@"<Root> <Child> Text </Child> </Root>");

            // Save XML with XML declaration.
            xmlTree.Save("root.xml");
            Console.WriteLine(File.ReadAllText("root.xml"));

            Console.WriteLine("---");

            // Save XML without XML declaration.
            using (var xmlWriter = XmlWriter.Create("root.xml", new XmlWriterSettings { OmitXmlDeclaration = true }))
                xmlTree.Save(xmlWriter);
            Console.WriteLine(File.ReadAllText("root.xml"));
        }

        static void DeserializeWithXmlReader()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML reader.
            var xmlReader = XmlReader.Create(new StringReader(@"<Root> <Child> Text </Child> </Root>"));

            // Create XML tree.
            var xmlTree = XElement.Load(xmlReader);

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }
    }
}
