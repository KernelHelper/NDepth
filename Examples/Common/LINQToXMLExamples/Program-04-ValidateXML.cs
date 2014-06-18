using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void ValidateWithDtd()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML document with DTD schema.
            const string document =
                @"<?xml version=""1.0""?>
                    <!--XML file using a DTD-->
                    <!DOCTYPE store [
                    <!ELEMENT store (item)*> 
                    <!ELEMENT item (name,dept,price)>
                    <!ATTLIST item type CDATA #REQUIRED>
                    <!ELEMENT name (#PCDATA)>
                    <!ELEMENT dept (#PCDATA)>
                    <!ELEMENT price (#PCDATA)>]>
                    <store>
                        <item type=""supplies"" ISBN=""2-3631-4"">
                            <name>paint</name>
                            <dept>tools</dept>
                            <price>16.95</price>
                        </item>
                    </store>";
            
            // Parse and save XML document.
            XDocument.Parse(document).Save("store.xml");

            bool errors;

            // Create XML reader settings.
            var xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                ValidationType = ValidationType.DTD
            };
            xmlReaderSettings.ValidationEventHandler += (sender, args) =>
            {
                Console.WriteLine("Validation Error: {0}", args.Message);
                errors = true;
            };

            // Create XML reader to read XML.
            using (var reader = XmlReader.Create("store.xml", xmlReaderSettings))
            {
                Console.WriteLine("Validating doc1...");
                errors = false;

                // Load XML file and get a corresponding XElement.
                XElement.Load(reader);

                // Show parsing results.
                Console.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");
            }
        }

        static void ValidateWithXsd()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XSD markup document.
            const string xsdMarkup =
                @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                    <xsd:element name='Root'>
                        <xsd:complexType>
                            <xsd:sequence>
                                <xsd:element name='Child1' minOccurs='1' maxOccurs='1'/>
                                <xsd:element name='Child2' minOccurs='1' maxOccurs='1'/>
                            </xsd:sequence>
                        </xsd:complexType>
                    </xsd:element>
                </xsd:schema>";

            // Create schema set.
            var schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

            // Create valid XML document.
            var doc1 = 
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", "content1"),
                        new XElement("Child2", "content1")));

            // Create invalid XML document.
            var doc2 = 
                new XDocument(
                    new XElement("Root",
                        new XElement("Child1", "content1"),
                        new XElement("Child3", "content1")));

            bool errors;

            // Validate the first XML document.
            Console.WriteLine("Validating doc1...");
            errors = false;
            doc1.Validate(schemas, (o, e) =>
            {
                Console.WriteLine("{0}", e.Message);
                errors = true;
            });
            Console.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");

            Console.WriteLine();

            // Validate the second XML document.
            Console.WriteLine("Validating doc2...");
            errors = false;
            doc2.Validate(schemas, (o, e) =>
            {
                Console.WriteLine("{0}", e.Message);
                errors = true;
            });
            Console.WriteLine("doc2 {0}", errors ? "did not validate" : "validated");
        }
    }
}
