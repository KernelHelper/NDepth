using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    public static partial class MyExtensions
    {
        private static string GetQName(XElement xe)
        {
            var prefix = xe.GetPrefixOfNamespace(xe.Name.Namespace);
            if ((xe.Name.Namespace == XNamespace.None) || (prefix == null))
                return xe.Name.LocalName;

            return prefix + ":" + xe.Name.LocalName;
        }

        private static string GetQName(XAttribute xa)
        {
            if (xa.Parent == null)
                throw new ArgumentNullException("xa");

            var prefix = xa.Parent.GetPrefixOfNamespace(xa.Name.Namespace);
            if ((xa.Name.Namespace == XNamespace.None) || (prefix == null))
                return xa.Name.ToString();

            return prefix + ":" + xa.Name.LocalName;
        }

        private static string NameWithPredicate(XElement el)
        {
            if ((el.Parent) != null && (el.Parent.Elements(el.Name).Count() != 1))
                return GetQName(el) + "[" + (el.ElementsBeforeSelf(el.Name).Count() + 1) + "]";

            return GetQName(el);
        }

        private static string StrCat<T>(this IEnumerable<T> source, string separator)
        {
            return source.Aggregate(new StringBuilder(),
                (sb, i) => sb.Append(i.ToString()).Append(separator), 
                s => s.ToString());
        }

        public static string GetXPath(this XObject xobj)
        {
            if (xobj.Parent == null)
            {
                var doc = xobj as XDocument;
                if (doc != null)
                    return ".";

                var el = xobj as XElement;
                if (el != null)
                    return "/" + NameWithPredicate(el);

                // The XPath data model does not include white space text nodes
                // that are children of a document, so this method returns null.
                var xt = xobj as XText;
                if (xt != null)
                    return null;

                var com = xobj as XComment;
                if ((com != null) && (com.Document != null))
                {
                    return
                        "/" +
                        (
                            com
                            .Document
                            .Nodes()
                            .OfType<XComment>()
                            .Count() != 1
                            ? "comment()[" +
                            (
                                com
                                .NodesBeforeSelf()
                                .OfType<XComment>()
                                .Count() + 1
                            ) + "]"
                            : "comment()"
                        );
                }
                var pi = xobj as XProcessingInstruction;
                if ((pi != null) && (pi.Document != null))
                {
                    return
                        "/" +
                        (
                            pi.Document.Nodes()
                            .OfType<XProcessingInstruction>()
                            .Count() != 1
                            ? "processing-instruction()[" +
                            (
                                pi
                                .NodesBeforeSelf()
                                .OfType<XProcessingInstruction>()
                                .Count() + 1
                            ) + "]"
                            : "processing-instruction()"
                        );
                }
                return null;
            }
            else
            {
                var el = xobj as XElement;
                if (el != null)
                {
                    return
                        "/" +
                        el
                        .Ancestors()
                        .InDocumentOrder()
                        .Select(NameWithPredicate)
                        .StrCat("/") +
                        NameWithPredicate(el);
                }
                var at = xobj as XAttribute;
                if ((at != null) && (at.Parent != null))
                {
                    return
                        "/" +
                        at
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(NameWithPredicate)
                        .StrCat("/") +
                        "@" + GetQName(at);
                }
                var com = xobj as XComment;
                if ((com != null) && (com.Parent != null))
                {
                    return
                        "/" +
                        com
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(NameWithPredicate)
                        .StrCat("/") +
                        (
                            com
                            .Parent
                            .Nodes()
                            .OfType<XComment>()
                            .Count() != 1
                            ? "comment()[" +
                            (
                                com
                                .NodesBeforeSelf()
                                .OfType<XComment>()
                                .Count() + 1
                            ) + "]"
                            : "comment()"
                        );
                }
                var cd = xobj as XCData;
                if ((cd != null) && (cd.Parent != null))
                {
                    return
                        "/" +
                        cd
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(NameWithPredicate)
                        .StrCat("/") +
                        (
                            cd
                            .Parent
                            .Nodes()
                            .OfType<XText>()
                            .Count() != 1
                            ? "text()[" +
                            (
                                cd
                                .NodesBeforeSelf()
                                .OfType<XText>()
                                .Count() + 1
                            ) + "]"
                            : "text()"
                        );
                }
                var tx = xobj as XText;
                if ((tx != null) && (tx.Parent != null))
                {
                    return
                        "/" +
                        tx
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(NameWithPredicate)
                        .StrCat("/") +
                        (
                            tx
                            .Parent
                            .Nodes()
                            .OfType<XText>()
                            .Count() != 1
                            ? "text()[" +
                            (
                                tx
                                .NodesBeforeSelf()
                                .OfType<XText>()
                                .Count() + 1
                            ) + "]"
                            : "text()"
                        );
                }
                var pi = xobj as XProcessingInstruction;
                if ((pi != null) && (pi.Parent != null))
                {
                    return
                        "/" +
                        pi
                        .Parent
                        .AncestorsAndSelf()
                        .InDocumentOrder()
                        .Select(NameWithPredicate)
                        .StrCat("/") +
                        (
                            pi
                            .Parent
                            .Nodes()
                            .OfType<XProcessingInstruction>()
                            .Count() != 1
                            ? "processing-instruction()[" +
                            (
                                pi
                                .NodesBeforeSelf()
                                .OfType<XProcessingInstruction>()
                                .Count() + 1
                            ) + "]"
                            : "processing-instruction()"
                        );
                }
                return null;
            }
        }
    }

    partial class Program
    {
        static void QueryBasedOnContext()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = XElement.Parse(
                @"<Root>
                    <p id=""1""/>
                    <ul>abc</ul>
                    <Child>
                        <p id=""2""/>
                        <notul/>
                        <p id=""3""/>
                        <ul>def</ul>
                        <p id=""4""/>
                    </Child>
                    <Child>
                        <p id=""5""/>
                        <notul/>
                        <p id=""6""/>
                        <ul>abc</ul>
                        <p id=""7""/>
                    </Child>
                </Root>");

            // Create LINQ query.
            var query =
                from e in xmlTree.Descendants("p")
                let z = e.ElementsAfterSelf().FirstOrDefault()
                where z != null && z.Name.LocalName == "ul"
                select e;

            // Show query results.
            Console.WriteLine("Show elements with next sibling 'ul':");
            foreach (var e in query)
                Console.WriteLine("id = {0}", (string)e.Attribute("id"));
        }

        static void QueryXPath()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = 
                new XElement("Root",
                    new XElement("Child1", 1),
                    new XElement("Child1", 2),
                    new XElement("Child1", 3),
                    new XElement("Child2", 4),
                    new XElement("Child2", 5),
                    new XElement("Child2", 6));

            // Create XPath query.
            var query = xmlTree.XPathSelectElements("./Child2");

            // Show query results.
            Console.WriteLine("Show Child2 elements:");
            foreach (var e in query)
                Console.WriteLine(e);
        }

        static void QueryXPathExpression()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML namespace.
            XNamespace aw = "http://www.adventure-works.com";

            // Create XML document.
            var document = 
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XProcessingInstruction("target", "data"),
                    new XElement("Root",
                        new XAttribute("AttName", "An Attribute"),
                        new XAttribute(XNamespace.Xmlns + "aw", aw.ToString()),
                        new XComment("This is a comment"),
                        new XElement("Child",
                            new XText("Text")),
                        new XElement("Child",
                            new XText("Other Text")),
                        new XElement("ChildWithMixedContent",
                            new XText("text"),
                            new XElement("b", "BoldText"),
                            new XText("otherText")),
                        new XElement(aw + "ElementInNamespace",
                            new XElement(aw + "ChildInNamespace"))));

            // Create LINQ query to get all descendant nodes.
            var query = document.DescendantNodes();

            // Show query results.
            Console.WriteLine("Show all descendant nodes:");
            foreach (var n in query)
            {
                Console.WriteLine(n.GetXPath());
                var el = n as XElement;
                if (el == null)
                    continue;
                
                foreach (var at in el.Attributes())
                    Console.WriteLine(at.GetXPath());
            }
        }
    }
}
