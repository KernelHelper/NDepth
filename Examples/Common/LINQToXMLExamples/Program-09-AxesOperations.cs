using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void GetCollectionOfElements()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Load XML file and get a corresponding XDocument.
            var document = XDocument.Load("books.xml");

            // This method retrieves a collection of the child elements of an element.
            var elements =
                from el in document.Elements().Elements()
                select el;

            // Show query results.
            Console.WriteLine("Show child elements:");
            foreach (var el in elements)
                Console.WriteLine((string)el.Element("Title"));
        }

        static void GetElementValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var strElement = new XElement("StringElement", "abcde");
            Console.WriteLine(strElement);
            Console.WriteLine("Value of element: " + (string)strElement);

            var intElement = new XElement("Age", "44");
            Console.WriteLine(intElement);
            Console.WriteLine("Value of element: " + (int)intElement);

            var valueElement = new XElement("StringElement", "abcde");
            Console.WriteLine(valueElement);
            Console.WriteLine("Value of element:" + valueElement.Value);
        }

        static void GetElementValueWithCheck()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = 
                new XElement("Root",
                    new XElement("Child1", "child 1 content"),
                    new XElement("Child2", "2"));

            // The following assignments show why it is easier to use
            // casting when the element might or might not exist.

            var c1 = (string)xmlTree.Element("Child1");
            Console.WriteLine("c1:{0}", c1 ?? "element does not exist");

            var c2 = (int?)xmlTree.Element("Child2");
            Console.WriteLine("c2:{0}", c2 == null ? "element does not exist" : c2.ToString());

            var c3 = (string)xmlTree.Element("Child3");
            Console.WriteLine("c3:{0}", c3 ?? "element does not exist");

            var c4 = (int?)xmlTree.Element("Child4");
            Console.WriteLine("c4:{0}", c4 == null ? "element does not exist" : c4.ToString());

            Console.WriteLine();

            // The following assignments show the required code when using
            // the Value property when the element might or might not exist.
            // Notice that this is more difficult than the casting approach.

            var e1 = xmlTree.Element("Child1");
            var v1 = (e1 == null) ? null : e1.Value;
            Console.WriteLine("v1:{0}", v1 ?? "element does not exist");

            var e2 = xmlTree.Element("Child2");
            int? v2;
            if (e2 == null)
                v2 = null;
            else
                v2 = Int32.Parse(e2.Value);
            Console.WriteLine("v2:{0}", v2 == null ? "element does not exist" : v2.ToString());

            var e3 = xmlTree.Element("Child3");
            var v3 = (e3 == null) ? null : e3.Value;
            Console.WriteLine("v3:{0}", v3 ?? "element does not exist");

            var e4 = xmlTree.Element("Child4");
            int? v4;
            if (e4 == null)
                v4 = null;
            else
                v4 = Int32.Parse(e4.Value);
            Console.WriteLine("v4:{0}", v4 == null ? "element does not exist" : v4.ToString());
        }

        static void GetDescendantsFilered()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Load XML file and get a corresponding XDocument.
            var document = XDocument.Load("books.xml");

            // This method retrieves a collection of the Title elements.
            var elements =
                from el in document.Descendants("Title")
                select el;

            // Show query results.
            Console.WriteLine("Show Title elements:");
            foreach (var el in elements)
                Console.WriteLine((string)el);
        }

        static void GetElementsChain()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Parse XML string and get a corresponding XElement.
            var xmlTree = XElement.Parse(
                @"<Root>
                    <ConfigParameter>RootConfigParameter</ConfigParameter>
                        <Customer>
                            <Name>Frank</Name>
                            <Config>
                                <ConfigParameter>FirstConfigParameter</ConfigParameter>
                            </Config>
                        </Customer>
                        <Customer>
                            <Name>Bob</Name>
                            <!--This customer doesn't have a Config element-->
                        </Customer>
                        <Customer>
                            <Name>Bill</Name>
                            <Config>
                                <ConfigParameter>SecondConfigParameter</ConfigParameter>
                            </Config>
                        </Customer>
                </Root>");

            // This method retrieves a collection of the ConfigParameter elements using LINQ query.
            var elements1 =
                from el in xmlTree.Elements("Customer").Elements("Config").Elements("ConfigParameter")
                select el;

            // Show query results.
            Console.WriteLine("Show ConfigParameter elements with LINQ:");
            foreach (var el in elements1)
                Console.WriteLine(el);

            // This method retrieves a collection of the ConfigParameter elements using fluent syntax.
            var elements2 = xmlTree.Elements("Customer").Elements("Config").Elements("ConfigParameter");

            // Show query results.
            Console.WriteLine("Show ConfigParameter elements with fluent syntax:");
            foreach (var el in elements2)
                Console.WriteLine(el);
        }

        static void GetFirstChildElement()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XElement("Child", "child 1 content"),
                    new XElement("Child", "2"));

            // This method retrieves the first child element from the document.
            var element = xmlTree.Element("Child");
            
            Console.WriteLine("First child element: " + (string)element);
        }

        static void GetAttributesCollection()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Value",
                    new XAttribute("ID", "1243"),
                    new XAttribute("Type", "int"),
                    new XAttribute("ConvertableTo", "double"),
                    "100");

            // This method retrieves collection of attributes from the current XML element.
            var attributes =
                from at in xmlTree.Attributes()
                select at;

            // Show query results.
            Console.WriteLine("Show attributes:");
            foreach (var at in attributes)
                Console.WriteLine(at);
        }

        static void GetAttributeValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("PhoneNumbers",
                    new XElement("Phone",
                        new XAttribute("type", "home"),
                        "555-555-5555"),
                    new XElement("Phone",
                        new XAttribute("type", "work"),
                        "555-555-6666"));

            // Create a query to get all Phone elements.
            var elements =
                from el in xmlTree.Descendants("Phone")
                select el;

            // Show query results.
            Console.WriteLine("Show all attributes:");
            foreach (var el in elements)
                Console.WriteLine((string)el.Attribute("type"));
        }
    }
}
