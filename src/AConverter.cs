using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.FileIO;
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
        /*
        private void MakeSourceListByTextFieldParser(CommonOptions opt)
        {
            opt.SourceList.Clear();
            TextFieldParser parser = new TextFieldParser(opt.File, System.IO.Text.Encoding.GetEncoding("UTF-8"));
            parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            parser.SetDelimiters(opt.Delimiter);
            while (false == parser.EndOfData) {
                opt.SourceList.append();
                string[] column = parser.ReadFields();
                for (int i = 0; i < column.Length; i++) {
                    opt.SourceList[-1].append(new Cell { Text=column[i] });
                }
            }
        }
        */
        abstract public string ToHtml();
    }
}
