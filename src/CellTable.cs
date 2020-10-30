using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NLog;
using CommandLine;
using CommandLine.Text;
namespace TsvToHtmlTable
{
    public class CellTable
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public List<List<Cell>> SourceList { get; private set; }
        public CellTable(List<List<Cell>> SourceList)
        {
            this.SourceList = SourceList;
        }
        public IEnumerable<(int r, int c)> Cells()
        {
            for (int r=0; r<this.SourceList.Count; r++)
            {
                for (int c=0; c<this.SourceList[r].Count; c++)
                {
                    yield return ( r:r, c:c );
                }
            }
        }
        public void SetBlankToZero()
        {
//            foreach (var i in this.Cells())
            foreach ((int r, int c) i in this.Cells())
            {
                if (0 == this.SourceList[i.r][i.c].Text.Length)
                {
                    this.SourceList[i.r][i.c].RowSpan = 0;
                    this.SourceList[i.r][i.c].ColSpan = 0;
                    logger.Debug("{},{} ({},{}) {}", i.r, i.c, this.SourceList[i.r][i.c].RowSpan, this.SourceList[i.r][i.c].ColSpan, this.SourceList[i.r][i.c].Text);
                } else {
                    this.SourceList[i.r][i.c].RowSpan = 1;
                    this.SourceList[i.r][i.c].ColSpan = 1;
                    logger.Debug("{},{} ({},{}) {}", i.r, i.c, this.SourceList[i.r][i.c].RowSpan, this.SourceList[i.r][i.c].ColSpan, this.SourceList[i.r][i.c].Text);
                }
            }
        }
        public int InferColumnHeaderCount()
        {
            if (0 == this.SourceList.Count) { return 0; }
            if (0 == this.SourceList[0].Count) { return 0; }
            int count = 0;
            foreach (var cell in this.SourceList[0])
            {
                if (0 == cell.Text.Length)
                {
                    count++;
                }
                else
                {
                    return count;
                }
            }
            return count;
        }
        public int InferRowHeaderCount()
        {
            if (0 == this.SourceList.Count) { return 0; }
//            bool[] has = new bool[this.SourceList[0].Count];
            List<bool> has = new List<bool>(this.SourceList[0].Count);
            foreach ((int r, int c) i in this.Cells())
            {
                if (0 != this.SourceList[i.r][i.c].Text.Length)
                {
                    has[i.c] = true;
                    if (has.All(v => true == v)) { return i.r + 1; }
                }
            }
            return this.SourceList.Count;
        }
    }
}
