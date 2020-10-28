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
                .WithParsed(opt => {/*パースに成功した場合*/ Run(opt); })
                .WithNotParsed(er => {/*パースに失敗した場合*/});
        }
        private static void Run(Options opt)
        {
            Console.WriteLine("CommandLine parse success!!");
//            Console.WriteLine(opt.A);
            Console.WriteLine($"A: {opt.A}");
            Console.WriteLine($"B: {opt.B}");
            Console.WriteLine($"C: {string.Join(", ", opt.C)}");
            Console.WriteLine($"D: {opt.D}");
            Console.WriteLine($"1: {string.Join(", ", opt.Remaining)}");

        }
    }
}
