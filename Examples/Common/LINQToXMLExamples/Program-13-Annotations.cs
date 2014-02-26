using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        public class MyAnnotation
        {
            public readonly string Tag;

            public MyAnnotation(string tag)
            {
                Tag = tag;
            }
        }

        static void Annotations()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child", 0));

            // Get root and child nodes.
            var root = xmlTree;
            var child = xmlTree.Element("Child");
            if (child == null)
                return;

            // Add annotations to the root and child elements.
            root.AddAnnotation(new MyAnnotation("MyRootAnno1"));
            root.AddAnnotation(new MyAnnotation("MyRootAnno2"));
            root.AddAnnotation("RootAnno1");
            root.AddAnnotation("RootAnno2");
            child.AddAnnotation(new MyAnnotation("MyChildAnno"));
            child.AddAnnotation("ChildAnno");

            // Create query to get all annotations of type MyAnnotation from the XML.
            var query1 =
                from el in xmlTree.DescendantsAndSelf()
                from anno in el.Annotations<MyAnnotation>()
                select anno.Tag;

            // Create query to get first string annotation from each element of the XML.
            var query2 =
                from el in xmlTree.DescendantsAndSelf()
                from anno in el.Annotations<string>()
                select anno;

            // Show query1 results.
            Console.WriteLine("Show MyAnnotation annotations:");
            foreach (var anno in query1)
                Console.WriteLine(anno);

            // Show query2 results.
            Console.WriteLine("Show string annotations:");
            foreach (var anno in query2)
                Console.WriteLine(anno);

            // Remove root string annotations.
            Console.WriteLine("Count of root string annotations before remove: " + root.Annotations<string>().Count());
            root.RemoveAnnotations<string>();
            Console.WriteLine("Count of root string annotations after remove: " + root.Annotations<string>().Count());

            // Remove child string annotations.
            Console.WriteLine("Count of root string annotations before remove: " + child.Annotations<string>().Count());
            child.RemoveAnnotations<string>();
            Console.WriteLine("Count of root string annotations after remove: " + child.Annotations<string>().Count());
        }
    }
}
