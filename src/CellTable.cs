using System;
using System.Text;
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
        public List<List<Cell>> DeepCopy(List<List<Cell>> cells)
        {
            List<List<Cell>> list = new List<List<Cell>>();
            foreach (var row in cells)
            {
                list.Add(new List<Cell>());
                foreach (var cell in row)
                {
                    list.Last().Add(cell.Clone());
                }
            }
            return list;
        }
        public void Log(List<List<Cell>> cells, LogLevel level=default!)
        {
            if (null == level) { level = LogLevel.Debug; }
            StringBuilder spanLog = new StringBuilder();
            StringBuilder textLog = new StringBuilder();
            for (int r=0; r<cells.Count; r++)
            {
                for (int c=0; c<cells[r].Count; c++)
                {
                    spanLog.Append($"({cells[r][c].RowSpan},{cells[r][c].ColSpan})\t");
                    textLog.Append($"{cells[r][c].Text}\t");
                }
                spanLog.Length--;
                textLog.Length--;
                spanLog.Append("\n");
                textLog.Append("\n");
            }
            logger.Log(level, "\n" + spanLog.ToString().TrimEnd('\n'));
            logger.Log(level, "\n" + textLog.ToString().TrimEnd('\n'));
        }

        public void SetBlankToZero()
        {
            foreach ((int r, int c) in this.Cells())
            {
                if (0 == this.SourceList[r][c].Text.Length)
                {
                    this.SourceList[r][c].RowSpan = 0;
                    this.SourceList[r][c].ColSpan = 0;
                } else {
                    this.SourceList[r][c].RowSpan = 1;
                    this.SourceList[r][c].ColSpan = 1;
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
            if (cells.Count <= r+len) { return len; }
            for (int R=r+len; R<cells.Count; R++)
            {
                if (0 == cells[R][c].Text.Length) { len++; }
                else { break; }
            }
            return len;
        }
        public int GetZeroLenByColumn(List<List<Cell>> cells, int r, int c)
        {
            int len = 1;
            if (cells[r].Count <= c+len) { return len; }
            for (int C=c+len; C<cells[r].Count; C++)
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
                    for (int R=r+1; R<r+cells[r][c].RowSpan; R++)
                    {
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
                    for (int C=c+1; C<c+cells[r][c].ColSpan; C++)
                    {
                        cells[r][C].RowSpan = -1;
                    }
                }
            }
            return cells;
        }
        public List<List<Cell>> SetZeroRect(List<List<Cell>> cells)
        {
            foreach ((int r, int c) in this.Cells(cells))
            {
                if (!IsZeroRect(cells, r, c, cells[r][c].RowSpan, cells[r][c].ColSpan))
                {
                    cells = SetSpanAllOne(cells, r, c, cells[r][c].RowSpan, cells[r][c].ColSpan);
                }
            }
            return cells;
        }
        private bool IsZeroRect(List<List<Cell>> cells, int r, int c, int rs, int cs)
        {
            if (1 < rs && 1 < cs)
            {
                for (int R=r+1; R<r+rs; R++)
                {
                    for (int C=c+1; C<c+cs; C++)
                    {
                        if (0 < cells[R][C].Text.Length)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private List<List<Cell>> SetSpanAllOne(List<List<Cell>> cells, int r, int c, int rs, int cs)
        {
            cells[r][c].RowSpan = 1;
            cells[r][c].ColSpan = 1;
            for (int R=r; R<r+rs; R++)
            {
                for (int C=c; C<c+cs; C++)
                {
                    if (cells[R][C].RowSpan < 1 && cells[R][C].ColSpan < 1)
                    {
                        cells[R][C].RowSpan = 1;
                        cells[R][C].ColSpan = 1;
                    }
                }
            }
            return cells;
        }
    }
}
