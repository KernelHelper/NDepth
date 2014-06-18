using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void CreateXElements()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ==="); 

            // Create XElement hierarchy using functional construction approach.
            var contacts = 
                new XElement("Contacts",
                    new XElement("Contact",
                        new XElement("Name", "Patrick Hines"),
                        new XElement("Phone", "206-555-0144"),
                        new XElement("Address",
                            new XElement("Street1", "123 Main St"),
                            new XElement("City", "Mercer Island"),
                            new XElement("State", "WA"),
                            new XElement("Postal", "68042"))));

            // Show XML hierarchy.
            Console.WriteLine(contacts);
        }

        static void CloneXElements()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XElement hierarchy using functional construction approach.
            var xmlTreeSource = 
                new XElement("Root",
                    new XElement("Element", 1),
                    new XElement("Element", 2),
                    new XElement("Element", 3),
                    new XElement("Element", 4),
                    new XElement("Element", 5));

            // Create another XElement hierarchy by cloning XML tree nodes.
            var xmlTreeClone = 
                new XElement("Root",
                    new XElement("Child", 1),
                    new XElement("Child", 2),
                    from el in xmlTreeSource.Elements()
                    where ((int)el > 2)
                    select el);

            // Show XML hierarchy.
            Console.WriteLine(xmlTreeClone);
        }

        static void CreateXAttributes()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XElement & XAttribute hierarchy using functional construction approach.
            var customers = 
                new XElement("Customers",
                    new XElement("Customer",
                        new XElement("Name", "John Doe"),
                        new XElement("PhoneNumbers",
                            new XElement("Phone",
                                new XAttribute("type", "home"), 
                                "555-555-5555"),
                            new XElement("Phone",
                                new XAttribute("type", "work"), 
                                "666-666-6666"))));

            // Show XML hierarchy.
            Console.WriteLine(customers);
        }

        static void CreateXDocument()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XDocument using functional construction approach.
            var books = 
                new XDocument(
                    new XComment("This is a comment."),
                    new XProcessingInstruction("xml-stylesheet", "href='mystyle.css' title='Compact' type='text/css'"),
                    new XElement("Pubs",
                        new XElement("Book",
                            new XElement("Title", "Artifacts of Roman Civilization"),
                            new XElement("Author", "Moreno, Jordao")),
                        new XElement("Book",
                            new XElement("Title", "Midieval Tools and Implements"),
                            new XElement("Author", "Gazit, Inbar"))),
                    new XComment("This is another comment."))
                    {
                        // Setup XML declaration for the document.
                        Declaration = new XDeclaration("1.0", "utf-8", "true")
                    };
            
            // Save XML document to file.
            books.Save("books.xml");

            // Show XML hierarchy.
            Console.WriteLine(books);
        }

        static void AttachVsClone()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create a tree with a child element.
            var xmlChild1 = 
                new XElement("Root", 
                    new XElement("Child1", 1));

            // Create an element that is not parented.
            var xmlChild2 = 
                new XElement("Child2", 2);

            // Create a tree and add xmlChild1 and xmlChild2 to it.
            var xmlTree = 
                new XElement("Root",
                    xmlChild1.Element("Child1"),
                    xmlChild2);

            // Compare xmlChild1 identity.
            Console.WriteLine("Child1 was {0}", (xmlTree.Element("Child1") == xmlChild1.Element("Child1")) ? "attached" : "cloned");

            // Compare xmlChild2 identity.
            Console.WriteLine("Child2 was {0}", (xmlTree.Element("Child2") == xmlChild2)  ? "attached" : "cloned");
        }
    }
}
