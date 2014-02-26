using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    partial class Program
    {
        static void TransformWithXslt()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create XSLT markup document.
            const string xslMarkup = 
                @"<?xml version='1.0'?>
                    <xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>
                        <xsl:template match='/Parent'>
                            <Root>
                                <C1>
                                    <xsl:value-of select='Child1'/>
                                </C1>
                                <C2>
                                    <xsl:value-of select='Child2'/>
                                </C2>
                            </Root>
                        </xsl:template>
                    </xsl:stylesheet>";

            // Create source XML document.
            var xmlSource = 
                new XDocument(
                    new XElement("Parent",
                        new XElement("Child1", "Child1 data"),
                        new XElement("Child2", "Child2 data")));

            // Perform XSLT transformation.
            var xmlDestination = new XDocument();
            using (var writer = xmlDestination.CreateWriter())
            {
                // Load the style sheet.
                var xslt = new XslCompiledTransform();
                xslt.Load(XmlReader.Create(new StringReader(xslMarkup)));

                // Execute the transformation and output the results to a writer.
                xslt.Transform(xmlSource.CreateReader(), writer);
            }

            // Show results.
            Console.WriteLine(xmlDestination);
        }
    }
}
