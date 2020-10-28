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
        }
        public override string ToHtml()
        {
            Console.WriteLine("InnerHeader.ToHtml()");
            return "<table></table>";
        }
    }
}
