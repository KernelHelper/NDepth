using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void ModifyXmlWithAdd()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create source XML tree.
            var xmlSource = 
                new XElement("Root",
                    new XElement("Element1", 1),
                    new XElement("Element2", 2),
                    new XElement("Element3", 3),
                    new XElement("Element4", 4),
                    new XElement("Element5", 5));

            // Create destination XML tree.
            var xmlDestination = 
                new XElement("Root",
                    new XElement("NewElement", "Content"));

            // Add source content as children of the destination one after the existing content.
            xmlDestination.Add(
                from el in xmlSource.Elements()
                where (int)el >= 3
                select el);

            // Show XML hierarchy.
            Console.WriteLine(xmlDestination);            
        }

        static void ModifyXmlWithAddFirst()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create source XML tree.
            var xmlSource =
                new XElement("Root",
                    new XElement("Element1", 1),
                    new XElement("Element2", 2),
                    new XElement("Element3", 3),
                    new XElement("Element4", 4),
                    new XElement("Element5", 5));

            // Create destination XML tree.
            var xmlDestination =
                new XElement("Root",
                    new XElement("NewElement", "Content"));

            // Add source content as children of the destination one first to the existing content.
            xmlDestination.AddFirst(
                from el in xmlSource.Elements()
                where (int)el >= 3
                select el);

            // Show XML hierarchy.
            Console.WriteLine(xmlDestination);
        }

        static void ModifyXmlWithAddAfterSelf()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create source XML tree.
            var xmlSource = 
                new XElement("Root",
                    new XElement("Element1", 1),
                    new XElement("Element2", 2),
                    new XElement("Element3", 3),
                    new XElement("Element4", 4),
                    new XElement("Element5", 5));

            // Create destination XML tree.
            var xmlDestination = 
                new XElement("Root",
                    new XElement("Child1", 1),
                    new XElement("Child2", 2),
                    new XElement("Child3", 3),
                    new XElement("Child4", 4),
                    new XElement("Child5", 5));

            // Get a required place to insert.
            var child1 = xmlDestination.Element("Child1");
            if (child1 == null)
                return;

            // Add source content as siblings of the destination one after the existing node.
            child1.AddAfterSelf(
                from el in xmlSource.Elements()
                where (int)el > 3
                select el);

            // Show XML hierarchy.
            Console.WriteLine(xmlDestination);
        }

        static void ModifyXmlWithAddBeforeSelf()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create source XML tree.
            var xmlSource =
                new XElement("Root",
                    new XElement("Element1", 1),
                    new XElement("Element2", 2),
                    new XElement("Element3", 3),
                    new XElement("Element4", 4),
                    new XElement("Element5", 5));

            // Create destination XML tree.
            var xmlDestination =
                new XElement("Root",
                    new XElement("Child1", 1),
                    new XElement("Child2", 2),
                    new XElement("Child3", 3),
                    new XElement("Child4", 4),
                    new XElement("Child5", 5));

            // Get a required place to insert.
            var child1 = xmlDestination.Element("Child1");
            if (child1 == null)
                return;

            // Add source content as siblings of the destination one before the existing node.
            child1.AddBeforeSelf(
                from el in xmlSource.Elements()
                where (int)el > 3
                select el);

            // Show XML hierarchy.
            Console.WriteLine(xmlDestination);
        }

        static void ModifyXmlWithReplaceAll()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Data", 1),
                    new XElement("Data", 2),
                    new XElement("Data", 3),
                    new XElement("Data", 4),
                    new XElement("Data", 5));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
            Console.WriteLine("-----");

            // Replace all child elements in the XML tree with a new ones.
            xmlTree.ReplaceAll(
                from el in xmlTree.Elements()
                where (int)el >= 3
                select new XElement("NewData", (int)el));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithReplaceAttributes()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree with one node containing some attributes.
            var xmlTree = 
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Data", 1),
                    new XElement("Data", 2),
                    new XElement("Data", 3),
                    new XElement("Data", 4),
                    new XElement("Data", 5));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
            Console.WriteLine("-----");

            // Replace all attributes in the XML node with a new one.
            xmlTree.ReplaceAttributes(new XAttribute("NewAtt1", 101));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithReplaceNodes()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Child", 1),
                    new XElement("Child", 2),
                    new XElement("Child", 3),
                    new XElement("Child", 4),
                    new XElement("Child", 5));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
            Console.WriteLine("-----");

            // Replace all child nodes in the XML tree with the specified content.
            xmlTree.ReplaceNodes(
                from el in xmlTree.Elements()
                where (int)el >= 3
                select el);

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithReplaceWith()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Child1", "child1 content"),
                    new XElement("Child2", "child2 content"),
                    new XElement("Child3", "child3 content"),
                    new XElement("Child4", "child4 content"),
                    new XElement("Child5", "child5 content"));

            // Get a required place to replace.
            var child3 = xmlTree.Element("Child3");
            if (child3 == null)
                return;

            // Replace the given child node with the specified content.
            child3.ReplaceWith(new XElement("NewChild", "new content"));

            // Show XML hierarchy.
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithRemove()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            XElement xmlChild;
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Child1", "child1 content"),
                    new XElement("Child", "child2 content"),
                    new XElement("Child", "child3 content"),
                    new XElement("Child", "child4 content"),
                    xmlChild = new XElement("Child5", "child5 content"));

            // Remove all Child elements.
            xmlTree.Elements("Child").Remove();
            Console.WriteLine(xmlTree);

            // Remove Child5 element.
            xmlChild.Remove();
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithRemoveAll()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Child1", "child1 content"),
                    new XElement("Child", "child2 content"),
                    new XElement("Child", "child3 content"),
                    new XElement("Child", "child4 content"),
                    new XElement("Child5", "child5 content"));

            // Remove all root elements and attributes.
            xmlTree.RemoveAll();
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithRemoveAttributes()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree =
                new XElement("Root",
                    new XAttribute("Att1", 1),
                    new XAttribute("Att2", 2),
                    new XAttribute("Att3", 3),
                    new XElement("Child1", "child1 content"),
                    new XElement("Child", "child2 content"),
                    new XElement("Child", "child3 content"),
                    new XElement("Child", "child4 content"),
                    new XElement("Child5", "child5 content"));

            // Remove all root attributes.
            xmlTree.RemoveAttributes();
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithElementSetValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = 
                new XElement("Root",
                    new XElement("Child", "child content"));

            // Set the root value with a string.
            xmlTree.SetValue("new content");
            Console.WriteLine(xmlTree);

            // Set the root value with a content string.
            xmlTree.Value = "new content";
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithAttributeSetValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = 
                new XElement("Root",
                    new XAttribute("Att1", "content1"),
                    new XAttribute("Att2", "content2"),
                    new XAttribute("Att3", "content3"));

            // Set the root value with a string.
            var att1 = xmlTree.Attribute("Att1");
            if (att1 == null)
                return;
            att1.SetValue("new content");
            Console.WriteLine(xmlTree);

            // Set the root value with a content string.
            var att2 = xmlTree.Attribute("Att2");
            if (att2 == null)
                return;
            att2.Value = "new content";
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithSetElementValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = new XElement("Root");

            // Add some name/value pairs.
            xmlTree.SetElementValue("Ele1", 1);
            xmlTree.SetElementValue("Ele2", 2);
            xmlTree.SetElementValue("Ele3", 3);
            Console.WriteLine(xmlTree);

            // Modify one of the name/value pairs.
            xmlTree.SetElementValue("Ele2", 22);
            Console.WriteLine(xmlTree);

            // Remove one of the name/value pairs.
            xmlTree.SetElementValue("Ele3", null);
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlWithSetAttributeValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML tree.
            var xmlTree = new XElement("Root");

            // Add some name/value pairs.
            xmlTree.SetAttributeValue("Att1", 1);
            xmlTree.SetAttributeValue("Att2", 2);
            xmlTree.SetAttributeValue("Att3", 3);
            Console.WriteLine(xmlTree);

            // Modify one of the name/value pairs.
            xmlTree.SetAttributeValue("Att2", 22);
            Console.WriteLine(xmlTree);

            // Remove one of the name/value pairs.
            xmlTree.SetAttributeValue("Att3", null);
            Console.WriteLine(xmlTree);
        }

        static void ModifyXmlTransformVsFunctional()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XML string.
            const string document = 
                @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                    <Root Data1=""123"" Data2=""456"">
                    <Child1>Content</Child1>
                </Root>";

            // Modify XML using transformation.
            var root1 = XElement.Parse(document);
            foreach (var att in root1.Attributes())
                root1.Add(new XElement(att.Name, (string)att));
            root1.Attributes().Remove();
            Console.WriteLine(root1);

            // Modify XML using functional construction approach.
            var root2 = XElement.Parse(document);
            var xmlTree = 
                new XElement("Root",
                    root2.Element("Child1"),
                    from att in root2.Attributes()
                    select new XElement(att.Name, (string)att));
            Console.WriteLine(xmlTree);
        }
    }
}
