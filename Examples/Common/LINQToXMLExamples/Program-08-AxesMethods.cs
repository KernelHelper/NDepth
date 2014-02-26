using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void XContainerElement()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns the first child XElement object that has the specified XName.

            // Create source XML tree.
            var xmlSourceTree =
                new XElement("Root",
                    new XElement("Element1", 1),
                    new XElement("Element2", 2),
                    new XElement("Element3", 3),
                    new XElement("Element4", 4),
                    new XElement("Element5", 5));

            // Create destination XML tree.
            var xmlDestinationTree =
                new XElement("Root",
                    new XElement("Child1", 1),
                    new XElement("Child2", 2),
                    new XElement("Child3", 3),
                    new XElement("Child4", 4),
                    new XElement("Child5", 5),
                    xmlSourceTree.Element("Element3"),
                    // Even though Element9 does not exist in srcTree, the following line
                    // will not throw an exception.
                    xmlSourceTree.Element("Element9"));

            // Show XML hierarchy.
            Console.WriteLine(xmlDestinationTree);
        }

        static void XContainerElements1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of the child elements of this element or document, in document order.

            // Create XML tree.
            var xmlTree = 
                new XElement("Root",
                    new XElement("Child1", 
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3", 
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5", 
                        new XElement("GrandChild5", 5)));

            // Create a query to get a collection of the child elements of this element or document, in document order.
            var elements =
                from el in xmlTree.Elements()
                select el;

            // Show query results.
            Console.WriteLine("Show child elements:");
            foreach (var el in elements)
                Console.WriteLine(el);
        }

        static void XContainerElements2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of the child elements of this element or document, 
            // in document order. Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child2",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child2",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Create a query to get a collection of the child elements with the given XName.
            var elements =
                from el in xmlTree.Elements("Child2")
                select el;

            // Show query results.
            Console.WriteLine("Show Child2 child elements:");
            foreach (var el in elements)
                Console.WriteLine(el);
        }

        static void XNodeElementsAfterSelf1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of the sibling elements after this node, in document order.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Get the GrandChild3 node.
            var grandchild = xmlTree.Element("Child3");
            if (grandchild == null)
                return;

            // Create a query to get a collection of the sibling elements after this node, in document order.
            var elements = grandchild.ElementsAfterSelf();

            // Show query results.
            Console.WriteLine("Show sibling elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XNodeElementsAfterSelf2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of the sibling elements after this node, in document order. 
            // Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child4",
                        new XElement("GrandChild5", 5)));

            // Get the GrandChild3 node.
            var grandchild = xmlTree.Element("Child2");
            if (grandchild == null)
                return;

            // Create a query to get a filtered collection of the sibling elements after this node, in document order.
            var elements = grandchild.ElementsAfterSelf("Child4");

            // Show query results.
            Console.WriteLine("Show Child4 sibling elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XNodeElementsBeforeSelf1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of the sibling elements before this node, in document order.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Get the GrandChild3 node.
            var grandchild = xmlTree.Element("Child3");
            if (grandchild == null)
                return;

            // Create a query to get a collection of the sibling elements before this node, in document order.
            var elements = grandchild.ElementsBeforeSelf();

            // Show query results.
            Console.WriteLine("Show sibling elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XNodeElementsBeforeSelf2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of the sibling elements before this node, in document order. 
            // Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child1",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Get the GrandChild4 node.
            var grandchild = xmlTree.Element("Child4");
            if (grandchild == null)
                return;

            // Create a query to get a filtered collection of the sibling elements before this node, in document order.
            var elements = grandchild.ElementsBeforeSelf("Child1");

            // Show query results.
            Console.WriteLine("Show Child1 sibling elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XNodeAncestors1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of the ancestor elements of this node.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Create a query to get GrandChild3 nodes.
            var grandchild =
                from el in xmlTree.Descendants("GrandChild3")
                select el;

            // Create a query to get a collection of the ancestor elements of this node.
            var elements = grandchild.Ancestors();

            // Show query results.
            Console.WriteLine("Show ancestor elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XNodeAncestors2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of the ancestor elements of this node. 
            // Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child",
                        new XElement("Child",
                            new XElement("GrandChild1", 1)),
                        new XElement("Child",
                            new XElement("GrandChild2", 2)),
                        new XElement("Child",
                            new XElement("GrandChild3", 3)),
                        new XElement("Child",
                            new XElement("GrandChild4", 4)),
                        new XElement("Child",
                            new XElement("GrandChild5", 5))));

            // Create a query to get GrandChild3 nodes.
            var grandchild =
                from el in xmlTree.Descendants("GrandChild3")
                select el;

            // Create a query to get a collection of the Child ancestor elements of this node.
            var elements = grandchild.Ancestors("Child");

            // Show query results.
            Console.WriteLine("Show Child ancestor elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XElementAncestorsAndSelf1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of elements that contain this element, and the ancestors of this element.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Create a query to get GrandChild3 nodes.
            var grandchild =
                from el in xmlTree.Descendants("GrandChild3")
                select el;

            // Create a query to get this node and a collection of the ancestor elements of this node.
            var elements = grandchild.AncestorsAndSelf();

            // Show query results.
            Console.WriteLine("Show current node and ancestor elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XElementAncestorsAndSelf2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of elements that contain this element, and the ancestors of this element. 
            // Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child",
                        new XElement("Child",
                            new XElement("GrandChild1", 1)),
                        new XElement("Child",
                            new XElement("GrandChild2", 2)),
                        new XElement("Child",
                            new XElement("GrandChild3", 3)),
                        new XElement("Child",
                            new XElement("GrandChild4", 4)),
                        new XElement("Child",
                            new XElement("GrandChild5", 5))));

            // Create a query to get GrandChild3 nodes.
            var grandchild =
                from el in xmlTree.Descendants("GrandChild3")
                select el;

            // Create a query to get this node and a collection of the Child ancestor elements of this node.
            var elements = grandchild.AncestorsAndSelf("Child");

            // Show query results.
            Console.WriteLine("Show Child ancestor elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XContainerDescendants1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of the descendant elements for this document or element, in document order.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Create a query to get a collection of the descendant elements for this document or element, in document order.
            var elements =
                from el in xmlTree.Descendants()
                select el;

            // Show query results.
            Console.WriteLine("Show descendant elements:");
            foreach (var el in elements)
                Console.WriteLine(el);
        }

        static void XContainerDescendants2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of the descendant elements for this document or element, 
            // in document order. Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("Child2", 2)),
                    new XElement("Child2",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child2",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("Child2", 5)));

            // Create a query to get a collection of the descendant elements with the given XName.
            var elements =
                from el in xmlTree.Descendants("Child2")
                select el;

            // Show query results.
            Console.WriteLine("Show Child2 descendant elements:");
            foreach (var el in elements)
                Console.WriteLine(el);
        }

        static void XElementDescendantsAndSelf1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of elements that contain this element, 
            // and all descendant elements of this element, in document order.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("GrandChild2", 2)),
                    new XElement("Child3",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child4",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("GrandChild5", 5)));

            // Create a query to get a collection of elements that contain this element, and all descendant elements of this element, in document order.
            var elements =
                from el in xmlTree.DescendantsAndSelf()
                select el;

            // Show query results.
            Console.WriteLine("Show descendant elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XElementDescendantsAndSelf2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of elements that contain this element, 
            // and all descendant elements of this element, in document order. 
            // Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child1",
                        new XElement("GrandChild1", 1)),
                    new XElement("Child2",
                        new XElement("Child2", 2)),
                    new XElement("Child2",
                        new XElement("GrandChild3", 3)),
                    new XElement("Child2",
                        new XElement("GrandChild4", 4)),
                    new XElement("Child5",
                        new XElement("Child2", 5)));

            // Create a query to get a filtered collection of elements that contain this element, 
            // and all descendant elements of this element, in document order.
            var elements =
                from el in xmlTree.DescendantsAndSelf("Child2")
                select el;

            // Show query results.
            Console.WriteLine("Show Child2 descendant elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XElementAttributes1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a collection of attributes of this element.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", "content1"),
                    new XAttribute("Att2", "content2"));

            // Create a query to get a collection of attributes of this element.
            var elements =
                from el in xmlTree.Attributes()
                select el;

            // Show query results.
            Console.WriteLine("Show attribute elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XElementAttributes2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns a filtered collection of attributes of this element. 
            // Only elements that have a matching XName are included in the collection.

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", "content1"),
                    new XAttribute("Att2", "content2"));

            // Create a query to get a collection of attributes of this element.
            var elements =
                from el in xmlTree.Attributes("Att1")
                select el;

            // Show query results.
            Console.WriteLine("Show Att1 attribute elements:");
            foreach (var el in elements)
                Console.WriteLine(el.Name);
        }

        static void XElementAttribute()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Returns the XAttribute that has the specified XName.

            // Create XML namespace.
            XNamespace aw = "http://www.adventure-works.com";

            // Create XML tree.
            var xmlTree =
                new XElement(aw + "Root",
                    new XAttribute(XNamespace.Xmlns + "aw", aw.NamespaceName),
                    new XAttribute(aw + "Att1", "content1"),
                    new XAttribute(aw + "Att2", "content2"));

            // Get attributes.
            var attribute1 = xmlTree.Attribute(aw + "Att2");
            // This will be null because required namespace is not provided.
            var attribute2 = xmlTree.Attribute("Att1");

            // Show query results.
            Console.WriteLine("Show attribute elements:");
            Console.WriteLine(attribute1);
            Console.WriteLine(attribute2);
        }
    }
}
