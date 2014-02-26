using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void ParseXmlString()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Parse XML string and get a corresponding XDocument.
            var document = XDocument.Parse(
                @"<Contacts>
                    <Contact>
                        <Name>Patrick Hines</Name>
                        <Phone Type=""home"">206-555-0144</Phone>
                        <Phone type=""work"">425-555-0145</Phone>
                        <Address>
                            <Street1>123 Main St</Street1>
                            <City>Mercer Island</City>
                            <State>WA</State>
                            <Postal>68042</Postal>
                        </Address>
                        <NetWorth>10</NetWorth>
                    </Contact>
                    <Contact>
                        <Name>Gretchen Rivas</Name>
                        <Phone Type=""mobile"">206-555-0163</Phone>
                        <Address>
                            <Street1>123 Main St</Street1>
                            <City>Mercer Island</City>
                            <State>WA</State>
                            <Postal>68042</Postal>
                        </Address>
                        <NetWorth>11</NetWorth>
                    </Contact>
                </Contacts>");

            // Show XML hierarchy.
            Console.WriteLine(document);
        }

        static void ParseXmlStringWithError()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Parse invalid XML string and handle the exception.
            try
            {
                var document = XDocument.Parse(
                    @"<Contacts>
                        <Contact>
                            <Name>Jim Wilson</Name>
                        </Contact>
                    </Contcts>");

                // Show XML hierarchy.
                Console.WriteLine(document);
            }
            catch (XmlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void LoadXmlFile()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Load XML file and get a corresponding XDocument.
            var document = XDocument.Load("books.xml");

            // Show XML hierarchy.
            Console.WriteLine(document);
        }

        static void LoadXmlFileWithOptions()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Load XML file and get a corresponding XDocument.
            var document = XDocument.Load("books.xml", LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            // Find a place to insert.
            var pubs = document.Element("Pubs");
            var book = (pubs != null) ? pubs.Element("Book") : null;
            if (book == null)
                return;

            // Add a node to the tree.
            // The newly added node will not have line information.
            book.AddAfterSelf(
                new XElement("Book",
                    new XElement("Title", "XML Developer's Guide"),
                    new XElement("Author", "Garghentini, Davide")));                

            // Show XML hierarchy information.
            var splitUri = document.BaseUri.Split('/');
            Console.WriteLine("BaseUri: {0}", splitUri[splitUri.Length - 1]);
            Console.WriteLine();
            Console.WriteLine("{0}{1}{2}",
                "Element Name".PadRight(20),
                "Line".PadRight(5),
                "Position");
            Console.WriteLine("{0}{1}{2}",
                "------------".PadRight(20),
                "----".PadRight(5),
                "--------");
            foreach (var e in pubs.DescendantsAndSelf())
                Console.WriteLine("{0}{1}{2}",
                    ("".PadRight(e.Ancestors().Count() * 2) + e.Name).PadRight(20),
                    ((IXmlLineInfo)e).HasLineInfo() ?
                        ((IXmlLineInfo)e).LineNumber.ToString(CultureInfo.InvariantCulture).PadRight(5) :
                        "",
                    ((IXmlLineInfo)e).HasLineInfo() ?
                        ((IXmlLineInfo)e).LinePosition.ToString(CultureInfo.InvariantCulture) :
                        "No Line Information");
        }

        static void LoadXmlReader()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XmlReader and read until the first element.
            using (var reader = XmlReader.Create("books.xml"))
            {
                while (reader.NodeType != XmlNodeType.Element)
                    reader.Read();

                // Load XML file and get a corresponding XElement.
                var root = XElement.Load(reader);

                // Show XML hierarchy.
                Console.WriteLine(root);
            }
        }

        static IEnumerable<XElement> StreamRootChildDoc(TextReader textReader)
        {
            using (var reader = XmlReader.Create(textReader))
            {
                // Skip unnecessary nodes.
                reader.MoveToContent();

                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Child")
                            {
                                var el = XElement.ReadFrom(reader) as XElement;
                                if (el != null)
                                    yield return el;
                            }
                            break;
                    }
                }
            }
        }

        static void LoadXmlStreamingReader()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            const string markup = 
                @"<Root>
                    <Child Key=""01"">
                        <GrandChild>aaa</GrandChild>
                    </Child>
                    <Child Key=""02"">
                        <GrandChild>bbb</GrandChild>
                    </Child>
                    <Child Key=""03"">
                        <GrandChild>ccc</GrandChild>
                    </Child>
                </Root>";

            // Create query which parses XML and collects GrandChild data.
            var grandChildData =
                from el in StreamRootChildDoc(new StringReader(markup))
                where (int)el.Attribute("Key") > 1
                select (string)el.Element("GrandChild");

            Console.WriteLine("Grand child:");
            foreach (var str in grandChildData)
            {
                Console.WriteLine(str);
            }
        }
    }
}
