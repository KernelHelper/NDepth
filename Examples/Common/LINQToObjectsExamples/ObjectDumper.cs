// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

using System;
using System.IO;
using System.Collections;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    public class ObjectDumper
    {
        public static void Write(object o)
        {
            Write(o, 0);
        }

        public static void Write(object o, int depth)
        {
            var dumper = new ObjectDumper(depth);
            dumper.WriteObject(null, o);
        }

        private readonly TextWriter _writer;
        private int _pos;
        private int _level;
        private readonly int _depth;

        private ObjectDumper(int depth)
        {
            _writer = Console.Out;
            _depth = depth;
        }

        private void Write(string s)
        {
            if (s == null) 
                return;

            _writer.Write(s);
            _pos += s.Length;
        }

        private void WriteIndent()
        {
            for (var i = 0; i < _level; i++) 
                _writer.Write("  ");
        }

        private void WriteLine()
        {
            _writer.WriteLine();
            _pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (_pos % 8 != 0) 
                Write(" ");
        }

        private void WriteObject(string prefix, object o)
        {
            if ((o == null) || (o is ValueType) || (o is string))
            {
                WriteIndent();
                Write(prefix);
                WriteValue(o);
                WriteLine();
            }
            else if (o is IEnumerable)
            {
                foreach (var element in (IEnumerable) o)
                {
                    if ((element is IEnumerable) && !(element is string))
                    {
                        WriteIndent();
                        Write(prefix);
                        Write("...");
                        WriteLine();
                        if (_level < _depth)
                        {
                            _level++;
                            WriteObject(prefix, element);
                            _level--;
                        }
                    }
                    else
                        WriteObject(prefix, element);
                }
            }
            else
            {
                MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                WriteIndent();
                Write(prefix);
                bool propWritten = false;
                foreach (var m in members)
                {
                    var f = m as FieldInfo;
                    var p = m as PropertyInfo;
                    if (f != null || p != null)
                    {
                        if (propWritten)
                        {
                            WriteTab();
                        }
                        else
                        {
                            propWritten = true;
                        }
                        Write(m.Name);
                        Write("=");
                        Type t = f != null ? f.FieldType : p.PropertyType;
                        if (t.IsValueType || (t == typeof(string)))
                            WriteValue(f != null ? f.GetValue(o) : p.GetValue(o, null));
                        else
                            Write(typeof (IEnumerable).IsAssignableFrom(t) ? "..." : "{ }");
                    }
                }
                if (propWritten) WriteLine();
                if (_level < _depth)
                {
                    foreach (MemberInfo m in members)
                    {
                        var f = m as FieldInfo;
                        var p = m as PropertyInfo;
                        if (f == null && p == null) 
                            continue;
                        var t = f != null ? f.FieldType : p.PropertyType;
                        if (t.IsValueType || (t == typeof (string))) 
                            continue;
                        var value = f != null ? f.GetValue(o) : p.GetValue(o, null);
                        if (value == null) 
                            continue;
                        _level++;
                        WriteObject(m.Name + ": ", value);
                        _level--;
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToShortDateString());
            }
            else if ((o is ValueType) || (o is string))
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
}

// ReSharper restore CanBeReplacedWithTryCastAndCheckForNull