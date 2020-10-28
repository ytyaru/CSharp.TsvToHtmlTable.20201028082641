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
        }
        public override string ToHtml()
        {
            Console.WriteLine("GroupHeader.ToHtml()");
            return "<table></table>";
        }
    }
}
