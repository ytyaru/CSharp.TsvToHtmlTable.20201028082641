using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public class NoneHeader : AConverter
    {
        public NoneHeader(NoneHeaderOptions opt):base()
        {
            Console.WriteLine("NoneHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
        }
        public override string ToHtml()
        {
            Console.WriteLine("NoneHeader.ToHtml()");
            return "<table></table>";
        }
    }
}
