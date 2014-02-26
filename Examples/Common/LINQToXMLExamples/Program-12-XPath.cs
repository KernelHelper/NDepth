using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void QueryXPathChildElement()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3)));

            // Create LINQ query.
            var element1 = xmlTree.Element("Child2");

            // Create XPath queries.
            var element2 = xmlTree.XPathSelectElement("Child2");
            var element3 = xmlTree.XPathSelectElement("./Child2");
            var element4 = xmlTree.XPathSelectElement("child::Child2");

            // Show query results.
            if ((element1 == element2) && (element2 == element3) && (element3 == element4))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            Console.WriteLine(element1);
        }

        static void QueryXPathChildElements()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3)));

            // Create LINQ query.
            var elements1 = xmlTree.Elements();

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("./*");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);                
        }

        static void QueryXPathRootElement()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3)));

            // Create LINQ query.
            var element1 = xmlTree.Root;

            // Create XPath query.
            var element2 = xmlTree.XPathSelectElement("/Root");

            // Show query results.
            if (element1 == element2)
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            Console.WriteLine(element1);
        }

        static void QueryXPathDescendants()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1",
                            new XElement("GrandChild", 1)),
                        new XElement("Child2",
                            new XElement("GrandChild", 2)),
                        new XElement("Child3",
                            new XElement("GrandChild", 3))));

            // Create LINQ query.
            var elements1 = xmlTree.Descendants("GrandChild");

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("//GrandChild");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);                
        }

        static void QueryXPathAttributeFilter()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1",
                            new XElement("GrandChild", 1, new XAttribute("Type", "Shipped"))),
                        new XElement("Child2",
                            new XElement("GrandChild", 2, new XAttribute("Type", "Shipped"))),
                        new XElement("Child3",
                            new XElement("GrandChild", 3, new XAttribute("Type", "Ordered")))));

            // Create LINQ query.
            var elements1 = 
                from el in xmlTree.Descendants("GrandChild")
                where (string)el.Attribute("Type") == "Shipped"
                select el;

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("//GrandChild[@Type='Shipped']");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);
        }

        static void QueryXPathRelatedElements()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Customers",
                            new XElement("Customer", "Customer 1", new XAttribute("Id", 1)),
                            new XElement("Customer", "Customer 2", new XAttribute("Id", 2)),
                            new XElement("Customer", "Customer 3", new XAttribute("Id", 3)),
                            new XElement("Customer", "Customer 4", new XAttribute("Id", 4))),
                        new XElement("Orders",
                            new XElement("Order", "Order 1", new XAttribute("CustomerId", 1)),
                            new XElement("Order", "Order 2", new XAttribute("CustomerId", 1)),
                            new XElement("Order", "Order 3", new XAttribute("CustomerId", 2)),
                            new XElement("Order", "Order 4", new XAttribute("CustomerId", 2)))));

            var root = xmlTree.Element("Root");
            var orders = (root != null) ? root.Element("Orders") : null;
            if (orders == null)
                return;

            // Create LINQ query.
            var element1 =
                (from el in xmlTree.Descendants("Customer")
                where (int)el.Attribute("Id") == (int)orders.Elements("Order").Skip(2).First().Attribute("CustomerId")
                select el).First();

            // Create XPath query.
            var element2 = xmlTree.XPathSelectElement(".//Customer[@Id=/Root/Orders/Order[3]/@CustomerId]");

            // Show query results.
            if (element1 == element2)
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            Console.WriteLine(element1);
        }

        static void QueryXPathNamespaces()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespaces.
            XNamespace aw = "http://www.adventure-works.com";
            XNamespace fc = "www.fourthcoffee.com";

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement(aw + "Root",
                        new XAttribute(XNamespace.Xmlns + "fc", fc.NamespaceName),
                        new XElement(fc + "Child",
                            new XElement(aw + "DifferentChild", "other content")),
                        new XElement(aw + "Child2", "c2 content"),
                        new XElement(fc + "Child3", "c3 content")));

            // Grab the reader.
            var reader = xmlTree.CreateReader();
            if (reader.NameTable == null)
                return;
            
            // Create namespace manager.
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("aw", aw.NamespaceName);
            namespaceManager.AddNamespace("fc", fc.NamespaceName);

            // Create LINQ query.
            var elements1 =
                from el in xmlTree.Descendants()
                where el.Name.Namespace == aw.NamespaceName
                select el;

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("//aw:*", namespaceManager);

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el.Name);
        }

        static void QueryXPathFollowingSibling()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3),
                        new XElement("Child4", 4),
                        new XElement("Child5", 5),
                        new XElement("Child6", 6),
                        new XElement("Child7", 7)));

            var root = xmlTree.Element("Root");
            var child = (root != null) ? root.Element("Child4") : null;
            if (child == null)
                return;

            // Create LINQ query.
            var elements1 = child.ElementsAfterSelf();

            // Create XPath query.
            var elements2 = child.XPathSelectElements("following-sibling::*");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el.Name);
        }

        static void QueryXPathPrecedingSibling()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3),
                        new XElement("Child4", 4),
                        new XElement("Child5", 5),
                        new XElement("Child6", 6),
                        new XElement("Child7", 7)));

            var root = xmlTree.Element("Root");
            var child = (root != null) ? root.Element("Child4") : null;
            if (child == null)
                return;

            // Create LINQ query.
            var elements1 = child.ElementsBeforeSelf();

            // Create XPath query.
            var elements2 = child.XPathSelectElements("preceding-sibling::*");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el.Name);
        }

        static void QueryXPathPrecedingSiblingEx()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3),
                        new XElement("Child4", 4),
                        new XElement("Child5", 5),
                        new XElement("Child6", 6),
                        new XElement("Child7", 7)));

            var root = xmlTree.Element("Root");
            var child = (root != null) ? root.Element("Child4") : null;
            if (child == null)
                return;

            // Create LINQ query.
            var element1 = child.ElementsBeforeSelf().Last();

            // Create XPath query.
            var element2 = child.XPathSelectElement("preceding-sibling::*[1]");

            // Show query results.
            if (element1 == element2)
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            Console.WriteLine(element1);
        }

        static void QueryXPathParent()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child", 2),
                        new XElement("Child3", 3),
                        new XElement("Child4", 4),
                        new XElement("Child5", 5),
                        new XElement("Child", 6),
                        new XElement("Child7", 7)));

            var root = xmlTree.Element("Root");
            var child = (root != null) ? root.Element("Child4") : null;
            if ((child == null) || (child.Parent == null))
                return;

            // Create LINQ query.
            var elements1 = child.Parent.Elements("Child");
            
            // Create XPath query.
            var elements2 = child.XPathSelectElements("../Child");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);
        }

        static void QueryXPathParentAttribute()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child", 1, new XAttribute("Id", 1),
                            new XElement("GrandChild1", 1)),
                        new XElement("Child", 2, new XAttribute("Id", 2),
                            new XElement("GrandChild2", 2)),
                        new XElement("Child", 3, new XAttribute("Id", 3),
                            new XElement("GrandChild3", 3))));

            var grandchild = xmlTree.Descendants("GrandChild2").First();
            if ((grandchild == null) || (grandchild.Parent == null))
                return;

            // Create LINQ query.
            var attribute1 = grandchild.Parent.Attribute("Id");

            // Create XPath query.
            var attribute2 = ((IEnumerable)grandchild.XPathEvaluate("../@Id")).Cast<XAttribute>().First();

            // Show query results.
            if (attribute1 == attribute2)
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            Console.WriteLine(attribute1);
        }

        static void QueryXPathSpecificAttribute()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", 1),
                        new XElement("Child2", 2),
                        new XElement("Child3", 3, new XAttribute("Select", "true")),
                        new XElement("Child4", 4),
                        new XElement("Child5", 5, new XAttribute("Select", "true")),
                        new XElement("Child6", 6),
                        new XElement("Child7", 7)));

            if (xmlTree.Root == null)
                return;            

            // Create LINQ query.
            var elements1 =
                from el in xmlTree.Root.Elements()
                where el.Attribute("Select") != null
                select el;

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("./Root/*[@Select]");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);
        }

        static void QueryXPathPosition()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Child", 1),
                        new XElement("Child", 2),
                        new XElement("Child", 3),
                        new XElement("Child", 4),
                        new XElement("Child", 5),
                        new XElement("Child", 6),
                        new XElement("Child", 7)));

            if (xmlTree.Root == null)
                return;

            // Create LINQ query.
            var elements1 = xmlTree.Root.Elements("Child").Skip(2).Take(3);

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("/Root/Child[position() > 2 and position() <= 5]");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);
        }

        static void QueryXPathText()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = XElement.Parse(
                @"<Root>
                    <Paragraph>
                        <Text>This is the start of</Text>
                    </Paragraph>
                    <Comment>
                        <Text>This comment is not part of the paragraph text.</Text>
                    </Comment>
                    <Paragraph>
                        <Annotation Emphasis='true'>
                            <Text> a sentence.</Text>
                        </Annotation>
                    </Paragraph>
                    <Paragraph>
                        <Text> This is a second sentence.</Text>
                    </Paragraph>
                </Root>");

            // Create LINQ query.
            var string1 = xmlTree.Elements("Paragraph").Descendants("Text").Select(s => s.Value).Aggregate(
                new StringBuilder(),
                (s, i) => s.Append(i),
                s => s.ToString());

            // Create XPath query.
            var string2 = ((IEnumerable)xmlTree.XPathEvaluate("./Paragraph//Text/text()")).Cast<XText>().Select(s => s.Value).Aggregate(
                new StringBuilder(),
                (s, i) => s.Append(i),
                s => s.ToString());

            // Show query results.
            if (string1 == string2)
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            Console.WriteLine(string1);
        }

        static void QueryXPathUnion()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XDocument(
                    new XElement("Root",
                        new XElement("Customers",
                            new XElement("Customer", "Customer 1"),
                            new XElement("Customer", "Customer 2"),
                            new XElement("Customer", "Customer 3"),
                            new XElement("Customer", "Customer 4")),
                        new XElement("Orders",
                            new XElement("Order", "Order 1"),
                            new XElement("Order", "Order 2"),
                            new XElement("Order", "Order 3"),
                            new XElement("Order", "Order 4"))));

            // Create LINQ query.
            var elements1 = xmlTree.Descendants("Order").Concat(xmlTree.Descendants("Customer")).InDocumentOrder();

            // Create XPath query.
            var elements2 = xmlTree.XPathSelectElements("//Order | //Customer");

            // Show query results.
            if (elements1.ToList().SequenceEqual(elements2.ToList()))
                Console.WriteLine("Results are identical");
            else
                Console.WriteLine("Results differ");
            foreach (var el in elements1)
                Console.WriteLine(el);
        }
    }
}
