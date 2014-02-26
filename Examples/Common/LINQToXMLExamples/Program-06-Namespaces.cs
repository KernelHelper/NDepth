using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void CreateDefaultNamespace()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespace.
            XNamespace aw = "http://www.adventure-works.com";

            // The following example creates a document with one namespace. 
            // By default, LINQ to XML serializes this document with a default namespace.

            // Create XML tree.
            var xmlTree = 
                new XElement(aw + "Root",
                    new XElement(aw + "Child1", "Child content within the namespace"),
                    new XElement("Child2", "Child content outside the namespace"));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void CreateNamespacePrefix()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespace.
            XNamespace aw = "http://www.adventure-works.com";

            // The following example creates a document with one namespace. 
            // It also creates an attribute that declares the namespace with a namespace prefix.
            // To create an attribute that declares a namespace with a prefix, you create an attribute 
            // where the name of the attribute is the namespace prefix, and this name is in the Xmlns namespace.
            // The value of this attribute is the URI of the namespace.

            // Create XML tree.
            var xmlTree = 
                new XElement(aw + "Root",
                    new XAttribute(XNamespace.Xmlns + "aw", aw.NamespaceName),
                    new XElement(aw + "Child", "child content"));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void CreateTwoNamespacesWithDefault()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespaces.
            XNamespace aw = "http://www.adventure-works.com";
            XNamespace fc = "www.fourthcoffee.com";

            // The following example shows the creation of a document that contains two namespaces.
            // One is the default namespace.Another is a namespace with a prefix.
            // By including namespace attributes in the root element, the namespaces are serialized 
            // so that http://www.adventure-works.com is the default namespace, and www.fourthcoffee.com 
            // is serialized with a prefix of "fc".To create an attribute that declares a default namespace, 
            // you create an attribute with the name "xmlns", without a namespace.
            // The value of the attribute is the default namespace URI.

            // Create XML tree.
            var xmlTree = 
                new XElement(aw + "Root",
                    new XAttribute(XNamespace.Xmlns + "fc", fc.NamespaceName),
                    new XElement(fc + "Child",
                        new XElement(aw + "DifferentChild", "other content")),
                    new XElement(aw + "Child2", "c2 content"),
                    new XElement(fc + "Child3", "c3 content"));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void CreateTwoNamespacesWithoutDefault()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespaces.
            XNamespace aw = "http://www.adventure-works.com";
            XNamespace fc = "www.fourthcoffee.com";

            // The following example creates a document that contains two namespaces, both with namespace prefixes.

            // Create XML tree.
            var xmlTree = 
                new XElement(aw + "Root",
                    new XAttribute(XNamespace.Xmlns + "aw", aw.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "fc", fc.NamespaceName),
                    new XElement(fc + "Child",
                        new XElement(aw + "DifferentChild", "other content")),
                    new XElement(aw + "Child2", "c2 content"),
                    new XElement(fc + "Child3", "c3 content"));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void CreateNamespaceExpanded()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Another way to accomplish the same result is to use expanded names 
            // instead of declaring and creating an XNamespace object.
            // This approach has performance implications. Each time you pass a string 
            // that contains an expanded name to LINQ to XML, LINQ to XML must parse the name, 
            // find the atomized namespace, and find the atomized name. This process takes CPU time.
            // If performance is important, you might want to declare and use an XNamespace object explicitly.

            // Create XML tree.
            var xmlTree = 
                new XElement("{http://www.adventure-works.com}Root",
                    new XAttribute(XNamespace.Xmlns + "aw", "http://www.adventure-works.com"),
                    new XElement("{http://www.adventure-works.com}Child", "child content"));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void QueryNamespace1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = XElement.Parse(
                @"<Root xmlns='http://www.adventure-works.com'>
                    <Child>1</Child>
                    <Child>2</Child>
                    <Child>3</Child>
                    <AnotherChild>4</AnotherChild>
                    <AnotherChild>5</AnotherChild>
                    <AnotherChild>6</AnotherChild>
                </Root>");

            // Create LINQ query.
            var query =
                from el in xmlTree.Elements("Child")
                select el;

            // Show query results.
            Console.WriteLine("Show child elements:");
            foreach (var c in query)
                Console.WriteLine((int)c);
        }

        static void QueryNamespace2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespace.
            XNamespace aw = "http://www.adventure-works.com";

            // Create XML tree.
            var xmlTree = XElement.Parse(
                @"<Root xmlns='http://www.adventure-works.com'>
                    <Child>1</Child>
                    <Child>2</Child>
                    <Child>3</Child>
                    <AnotherChild>4</AnotherChild>
                    <AnotherChild>5</AnotherChild>
                    <AnotherChild>6</AnotherChild>
                </Root>");

            // Create LINQ query.
            var query =
                from el in xmlTree.Elements(aw + "Child")
                select el;

            // Show query results.
            Console.WriteLine("Show child elements:");
            foreach (var c in query)
                Console.WriteLine((int)c);
        }

        static void QueryNamespace3()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespace.
            XNamespace aw = "http://www.adventure-works.com";

            // Create XML tree.
            var xmlTree = XElement.Parse(
                @"<aw:Root xmlns:aw='http://www.adventure-works.com'>
                    <aw:Child>1</aw:Child>
                    <aw:Child>2</aw:Child>
                    <aw:Child>3</aw:Child>
                    <aw:AnotherChild>4</aw:AnotherChild>
                    <aw:AnotherChild>5</aw:AnotherChild>
                    <aw:AnotherChild>6</aw:AnotherChild>
                </aw:Root>");

            // Create LINQ query.
            var query =
                from el in xmlTree.Elements(aw + "Child")
                select el;

            // Show query results.
            Console.WriteLine("Show child elements:");
            foreach (var c in query)
                Console.WriteLine((int)c);
        }

        static void QueryNamespace4()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespaces.
            XNamespace aw = "http://www.adventure-works.com";
            XNamespace fc = "www.fourthcoffee.com";

            // The following example creates a document that contains two namespaces, both with namespace prefixes.

            // Create XML tree.
            var xmlTree =
                new XElement(aw + "Root",
                    new XAttribute(XNamespace.Xmlns + "aw", aw.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "fc", fc.NamespaceName),
                    new XElement(fc + "Child",
                        new XElement(aw + "DifferentChild", "other content")),
                    new XElement(aw + "Child2", "c2 content"),
                    new XElement(fc + "Child3", "c3 content"));

            // Create LINQ query.
            var query =
                from el in xmlTree.Elements()
                where el.Name.Namespace == aw
                select el;

            // Show query results.
            Console.WriteLine("Show child elements from the Adventure Works namespace:");
            foreach (var c in query)
                Console.WriteLine(c.Name + " = " + (string)c);
        }

        static void ChangeNamespace()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create the first XML tree.
            var tree1 = 
                new XElement("Data",
                    new XElement("Child", "content",
                        new XAttribute("MyAttr", "content")));

            // Create the second XML tree.
            var tree2 = new XElement("Data",
                new XElement("Child", "content",
                    new XAttribute("MyAttr", "content")));

            // Create XML namespaces.
            XNamespace aw = "http://www.adventure-works.com";
            XNamespace ad = "http://www.adatum.com";

            // Change the namespace of every element and attribute in the first tree.
            foreach (var el in tree1.DescendantsAndSelf())
            {
                el.Name = aw.GetName(el.Name.LocalName);
                List<XAttribute> atList = el.Attributes().ToList();
                el.Attributes().Remove();
                foreach (XAttribute at in atList)
                    el.Add(new XAttribute(aw.GetName(at.Name.LocalName), at.Value));
            }

            // Change the namespace of every element and attribute in the second tree.
            foreach (var el in tree2.DescendantsAndSelf())
            {
                el.Name = ad.GetName(el.Name.LocalName);
                List<XAttribute> atList = el.Attributes().ToList();
                el.Attributes().Remove();
                foreach (XAttribute at in atList)
                    el.Add(new XAttribute(ad.GetName(at.Name.LocalName), at.Value));
            }

            // Add attribute namespaces so that the tree will be serialized with the aw namespace prefix.
            tree1.Add(new XAttribute(XNamespace.Xmlns + "aw", "http://www.adventure-works.com"));

            // Add attribute namespaces so that the tree will be serialized with the ad namespace prefix.
            tree2.Add(new XAttribute(XNamespace.Xmlns + "ad", "http://www.adatum.com"));

            // Create a new composite tree.
            var root = 
                new XElement("Root",
                    tree1,
                    tree2);

            // Show XML hierarchy.
            Console.WriteLine(root);
        }
    }
}
