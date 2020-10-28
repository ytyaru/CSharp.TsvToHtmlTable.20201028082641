using System;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GroupHeaderOptions, InnerHeaderOptions, NoneHeaderOptions>(args)
                .WithParsed<GroupHeaderOptions>(opt => { Console.WriteLine(new GroupHeader(opt).ToHtml()); })
                .WithParsed<InnerHeaderOptions>(opt => { Console.WriteLine(new InnerHeader(opt).ToHtml()); })
                .WithParsed<NoneHeaderOptions>(opt => { Console.WriteLine(new NoneHeader(opt).ToHtml()); })
                .WithNotParsed(er => {  });
        }
    }
}
