using System;
using System.Data;

namespace NDepth.Examples.Database.SQLExamples
{
    public static class ExtensionMethods
    {
        public static void PrintToConsole(this DataTable dt)
        {
            const int columnWidth = 25;
            var tableWidth = (columnWidth * dt.Columns.Count) + dt.Columns.Count;

            ResizeTheWindow(tableWidth + 2);

            Console.WriteLine("");
            Console.WriteLine("Table name : " + dt.TableName + "\n");

            #region PRINT THE TABLE HEADER

            DrawHorizontalSeperator(tableWidth, '=');
            Console.Write("|");

            foreach (DataColumn column in dt.Columns)
            {
                string name = (" " + column.ColumnName + " ").PadRight(columnWidth);
                Console.Write(name + "|");
            }
            Console.WriteLine("");
            DrawHorizontalSeperator(tableWidth, '=');

            #endregion

            #region PRINTING DATA ROWS

            foreach (DataRow row in dt.Rows)
            {
                Console.Write("|");
                foreach (DataColumn column in dt.Columns)
                {
                    string value = (" " + GetShortString(row[column.ColumnName].ToString(), columnWidth) + " ").PadRight(columnWidth);
                    Console.Write(value + "|");
                }
                Console.WriteLine("");
                DrawHorizontalSeperator(tableWidth, '-');
            }

            #endregion

            Console.WriteLine("");
        }

        private static void ResizeTheWindow(int tableWidth)
        {
            if (tableWidth > Console.LargestWindowWidth)
            {
                Console.WindowWidth = Console.LargestWindowWidth;
                Console.SetWindowPosition(0, 0);
            }
            else if (tableWidth > Console.WindowWidth)
            {
                Console.WindowWidth = tableWidth;
            }
        }

        private static void DrawHorizontalSeperator(int width, char seperator)
        {
            for (var counter = 0; counter <= width; counter++)
            {
                Console.Write(seperator);
            }
            Console.WriteLine("");
        }

        private static string GetShortString(string text, int length)
        {
            return (text.Length >= length - 1) ? text.Substring(0, length - 5) + "..." : text;
        }
    }
}
