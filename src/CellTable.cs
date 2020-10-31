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
        public IEnumerable<(int r, int c)> Cells(List<List<Cell>> cells)
        {
            for (int r=0; r<cells.Count; r++)
            {
                for (int c=0; c<cells[r].Count; c++)
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
                    logger.Trace("{},{} ({},{}) {}", r, c, this.SourceList[r][c].RowSpan, this.SourceList[r][c].ColSpan, this.SourceList[r][c].Text);
                } else {
                    this.SourceList[r][c].RowSpan = 1;
                    this.SourceList[r][c].ColSpan = 1;
                    logger.Trace("{},{} ({},{}) {}", r, c, this.SourceList[r][c].RowSpan, this.SourceList[r][c].ColSpan, this.SourceList[r][c].Text);
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
            return InferRowHeaderCount(this.InferColumnHeaderCount());
        }
        public int InferRowHeaderCount(int columnHeaderCount)
        {
            logger.Trace("columnHeaderCount: {}", columnHeaderCount);
            if (0 == this.SourceList.Count) { return 0; }
            List<bool> has = new List<bool>(new bool[this.SourceList[0].Count - columnHeaderCount]);
            foreach ((int r, int c) in this.Cells())
            {
                if (c < columnHeaderCount) { continue; }
                if (0 != this.SourceList[r][c].Text.Length)
                {
                    has[c - columnHeaderCount] = true;
                    if (has.All(v => true == v)) { return r + 1; }
                }
            }
            return this.SourceList.Count;
        }
        public int GetZeroLenByRow(List<List<Cell>> cells, int r, int c)
        {
            int len = 1;
            if (cells.Count <= r+1+len) { return len; }
            for (int R=r+1; R<cells.Count; R++)
            {
                if (0 == cells[R][c].Text.Length) { len++; }
                else { break; }
            }
            return len;
        }
        public int GetZeroLenByColumn(List<List<Cell>> cells, int r, int c)
        {
            int len = 1;
            if (cells[r].Count <= c+1+len) { return len; }
            for (int C=c+1; C<cells[r].Count; C++)
            {
                if (0 == cells[r][C].Text.Length) { len++; }
                else { break; }
            }
            return len;
        }
        public List<List<Cell>> StopColSpanByRowSpan(List<List<Cell>> cells)
        {
            foreach ((int r, int c) in this.Cells(cells))
            {
                if (1 < cells[r][c].RowSpan)
                {
                    Console.WriteLine($"{r},{c}");
                    for (int R=r+1; R<r+cells[r][c].RowSpan; R++)
                    {
                        Console.WriteLine($"  {R},{c}");
                        cells[R][c].ColSpan = -1;
                    }
                }
            }
            return cells;
        }
        public List<List<Cell>> StopRowSpanByColSpan(List<List<Cell>> cells)
        {
            foreach ((int r, int c) in this.Cells(cells))
            {
                if (1 < cells[r][c].ColSpan)
                {
                    Console.WriteLine($"{r},{c}");
                    for (int C=c+1; C<c+cells[r][c].ColSpan; C++)
                    {
                        Console.WriteLine($"  {r},{C}");
                        cells[r][C].RowSpan = -1;
                    }
                }
            }
            return cells;
        }
    }
}
