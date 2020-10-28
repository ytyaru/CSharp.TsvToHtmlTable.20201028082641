using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public class InnerHeader : AConverter
    {
        public InnerHeader(InnerHeaderOptions opt):base()
        {
            Console.WriteLine("InnerHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine($"Header: {opt.Header}");
            Console.WriteLine($"Start: {opt.Start}");
            Console.WriteLine($"Step: {opt.Step}");
            Console.WriteLine($"HeaderAttributes: {opt.HeaderAttributes}");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
        }
        public override string ToHtml()
        {
            Console.WriteLine("InnerHeader.ToHtml()");
            return "<table></table>";
        }
    }
}
