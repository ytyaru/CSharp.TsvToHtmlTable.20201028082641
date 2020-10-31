using System;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public class Cell
    {
        public string Text { get; set; } = "";
        public int RowSpan { get; set; } = 0;
        public int ColSpan { get; set; } = 0;
        public Cell Clone()
        {
            return new Cell { Text=this.Text, RowSpan=this.RowSpan, ColSpan=this.ColSpan };
        }
    }
}
