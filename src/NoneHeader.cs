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
        }
        public override string ToHtml()
        {
            Console.WriteLine("NoneHeader.ToHtml()");
            return "<table></table>";
        }
    }
}
