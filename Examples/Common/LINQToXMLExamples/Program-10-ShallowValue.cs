using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace NDepth.Examples.Common.LINQToXMLExamples
{
    public static partial class MyExtensions
    {
        public static string ShallowValue(this XElement xe)
        {
            return xe.Nodes().OfType<XText>().Aggregate(new StringBuilder(), (s, c) => s.Append(c), s => s.ToString());
        }
    }

    partial class Program
    {
        static void GetShallowValue()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Parse XML string and get a corresponding XElement.
            var xmlTree = XElement.Parse(
                @"<Report>
                    <Section>
                        <Heading>
                            <Column Name='CustomerId'>=Customer.CustomerId.Heading</Column>
                            <Column Name='Name'>=Customer.Name.Heading</Column>
                        </Heading>
                        <Detail>
                            <Column Name='CustomerId'>=Customer.CustomerId</Column>
                            <Column Name='Name'>=Customer.Name</Column>
                        </Detail>
                    </Section>
                </Report>");

            // Create a query to get all required Column elements.
            var elements =
                from el in xmlTree.Descendants()
                where el.ShallowValue().StartsWith("=")
                select el;

            // Show query results.
            Console.WriteLine("Show elements with shallow values:");
            foreach (var el in elements)
            {
                var at = el.Attribute("Name");
                if (at == null)
                    return;

                Console.WriteLine("{0}{1}{2}",
                    el.Name.ToString().PadRight(8),
                    at.ToString().PadRight(20),
                    el.ShallowValue());                
            }
        }
    }
}
