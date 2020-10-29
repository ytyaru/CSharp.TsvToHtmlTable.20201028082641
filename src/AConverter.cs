using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.FileIO;
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
//            var cells = new List<List<Cell>>();
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
        abstract public string ToHtml();
    }
}
