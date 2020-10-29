using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace TsvToHtmlTable
{
    public abstract class AConverter
    {
        public AConverter(CommonOptions opt)
        {
            opt.SetLoggingLevels();
            opt.GetInput();
            this.MakeSourceList(opt);
        }
        private void MakeSourceList(CommonOptions opt)
        {
            opt.SourceList.Clear();
            using (opt.Source) {
                string line = "";
                while((line = opt.Source.ReadLine()) != null)  
                {  
                    System.Console.WriteLine(line);
                    opt.SourceList.Add(new List<Cell>());
                    foreach (string col in line.Split(opt.Delimiter))
                    {
                        opt.SourceList.Last().Add(new Cell { Text=col });
                    }
                }  
            }
            Console.WriteLine(opt.SourceList.Count);
            for (int r=0; r<opt.SourceList.Count; r++) {
                for (int c=0; c<opt.SourceList[r].Count; c++) {
                    Console.Write($"{opt.SourceList[r][c].Text}\t");
                }
                Console.WriteLine();
            }
        }
        abstract public string ToHtml();
    }
}
