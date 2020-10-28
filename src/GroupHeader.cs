using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public class GroupHeader : AConverter
    {
        public GroupHeader(GroupHeaderOptions opt):base()
        {
            Console.WriteLine("GroupHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine($"Header: {opt.Header}");
            Console.WriteLine($"Row: {opt.Row}");
            Console.WriteLine($"Column: {opt.Column}");
            Console.WriteLine($"RowHeaderAttributes: {opt.RowHeaderAttributes}");
            Console.WriteLine($"ColumnHeaderAttributes: {opt.ColumnHeaderAttributes}");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
        }
        public override string ToHtml()
        {
            Console.WriteLine("GroupHeader.ToHtml()");
            return "<table></table>";
        }
    }
}
