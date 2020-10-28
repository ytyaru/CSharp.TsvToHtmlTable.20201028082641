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
            Console.WriteLine("Hello world !!");
            Sub sub = new Sub();
            sub.Method();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opt => {/*パースに成功した場合*/})
                .WithNotParsed(er => {/*パースに失敗した場合*/});
        }
    }
}
