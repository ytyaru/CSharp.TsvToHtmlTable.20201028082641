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
            foreach ((int r, int c) in this.Cells())
            {
                if (0 == this.SourceList[r][c].Text.Length)
                {
                    this.SourceList[r][c].RowSpan = 0;
                    this.SourceList[r][c].ColSpan = 0;
                    logger.Debug("{},{} ({},{}) {}", r, c, this.SourceList[r][c].RowSpan, this.SourceList[r][c].ColSpan, this.SourceList[r][c].Text);
                } else {
                    this.SourceList[r][c].RowSpan = 1;
                    this.SourceList[r][c].ColSpan = 1;
                    logger.Debug("{},{} ({},{}) {}", r, c, this.SourceList[r][c].RowSpan, this.SourceList[r][c].ColSpan, this.SourceList[r][c].Text);
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
            int colHeadCnt = this.InferColumnHeaderCount();
            logger.Debug("colHeadCnt: {}", colHeadCnt);
            if (0 == this.SourceList.Count) { return 0; }
            List<bool> has = new List<bool>(new bool[this.SourceList[0].Count - colHeadCnt]);
            foreach ((int r, int c) in this.Cells())
            {
                if (c < colHeadCnt) { continue; }
                if (0 != this.SourceList[r][c].Text.Length)
                {
                    has[c - colHeadCnt] = true;
                    if (has.All(v => true == v)) { return r + 1; }
                }
            }
            return this.SourceList.Count;
        }
    }
}
