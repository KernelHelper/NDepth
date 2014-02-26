using System;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void Events()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = 
                new XElement("Root",
                     new XAttribute("Att", "att value"),
                     "root content");

            // Attach event handler to the root node.
            xmlTree.Changing += XmlTreeChanging;
            xmlTree.Changed += XmlTreeChanged;

            // Change the root node value.
            xmlTree.Value = "new root content";

            // Add new child element to the root node.
            xmlTree.Add(new XElement("Child", "child content"));

            // Remove child element from the root node.
            xmlTree.Elements("Child").Remove();

            // Rename the root node.
            xmlTree.Name = "MyRoot";

            // Change the root node attribute.
            xmlTree.FirstAttribute.Value = "new attribute";
        }

        static void XmlTreeChanging(object sender, XObjectChangeEventArgs e)
        {
            Console.WriteLine("Changing event raised");
            Console.WriteLine("  Sender: " + sender);
            Console.WriteLine("  Sender type: " + sender.GetType());
            Console.WriteLine("  Changing: " + e.ObjectChange);
        }

        static void XmlTreeChanged(object sender, XObjectChangeEventArgs e)
        {
            Console.WriteLine("Changed event raised");
            Console.WriteLine("  Sender: " + sender);
            Console.WriteLine("  Sender type: " + sender.GetType());
            Console.WriteLine("  Changed: " + e.ObjectChange);
        }
    }
}
