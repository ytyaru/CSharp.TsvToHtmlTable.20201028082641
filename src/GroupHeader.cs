using System;
//using System.Diagnostics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using NLog;
namespace TsvToHtmlTable
{
    public class GroupHeader : AConverter
    {
//        private Logger logger = LogManager.GetCurrentClassLogger();
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        public GroupHeader(GroupHeaderOptions opt):base(opt)
        {
            this.Options = opt;
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            try
            {
                logger.Debug("GroupHeader.ToHtml()");
                var cellTable = new CellTable(this.Options.SourceList);
                cellTable.SetBlankToZero();
                var header = new Header(this.Options);
                header.Infer(cellTable);
                throw new Exception("Some Error");
                return "<table></table>";
            }
            catch (Exception e)
            {
                Logger logger = NLog.LogManager.GetLogger("AppErrorLogger");
                logger.Error(e, "Error !!");
            }
            return "<table></table>";
        }
        private void ShowArgsNLog(GroupHeaderOptions opt)
        {
            logger.Debug("GroupHeader.ShowArgsNLog()");
            logger.Fatal("Fatal");
            logger.Error("Error");
            logger.Warn("Warn");
            logger.Info("Info");
            logger.Debug("Debug");
            logger.Trace("Trace");
            logger.Debug("----- Arguments -----");
            logger.Debug("Header                : {}", opt.Header);
            logger.Debug("Row                   : {}", opt.Row);
            logger.Debug("Column                : {}", opt.Column);
            logger.Debug("RowHeaderAttributes   : {}", opt.RowHeaderAttributes);
            logger.Debug("ColumnHeaderAttributes: {}", opt.ColumnHeaderAttributes);
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttributes       : {}", opt.TableAttributes);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("Source                :\n{}", opt.Source);
            logger.Debug("----------");
        }
    }
    class Header
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        public RowHeader Row { get; private set; } = default!;
        public ColumnHeader Column { get; private set; } = default!;
        public MatrixHeader Matrix { get; private set; } = default!;
        public Header(GroupHeaderOptions opt)
        {
            this.Options = opt;
        }
        public void Infer(CellTable cellTable)
        {
            int colCnt = cellTable.InferColumnHeaderCount();
            int rowCnt = cellTable.InferRowHeaderCount(colCnt);
            logger.Debug("InferColumnHeaderCount: {}", colCnt);
            logger.Debug("InferRowHeaderCount: {}", rowCnt);

//            bool hasReverseRowHeader = () ? :;

            Row = new RowHeader(this.Options, cellTable, rowCnt, colCnt);
            Column = new ColumnHeader(this.Options, cellTable, rowCnt, colCnt);
            Matrix = new MatrixHeader(this.Options, rowCnt, colCnt);
        }

    }
    class RowHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        public CellTable CellTable { get; private set; }
        public int Count { get; private set; }
        public List<List<Cell>> Cells { get; private set; } = new List<List<Cell>>();
        public List<List<Cell>> ReversedCells { get; private set; } = new List<List<Cell>>();
        private int RowHeaderCount;
        private int ColumnHeaderCount;
        public RowHeader(GroupHeaderOptions opt, CellTable cellTable, int rowCnt, int colCnt, bool hasReverseHeader=false)
        {
            this.Options = opt;
            this.CellTable = cellTable;
            this.RowHeaderCount = rowCnt;
            this.ColumnHeaderCount = rowCnt;
            logger.Trace("colCnt: {}", colCnt);
            MakeCells(rowCnt, colCnt);
            logger.Debug("RowHeader");
            if (RowHeaderPosType.b == this.Options.Row || 
                RowHeaderPosType.B == this.Options.Row) { MakeReversedCells(); }
        }
        private void MakeCells(int rowCnt, int colCnt)
        {
            foreach (var row in this.Options.SourceList.GetRange(0, rowCnt))
            {
                logger.Trace("row: {}", row);
                this.Cells.Add(row.GetRange(colCnt, row.Count - colCnt));
                logger.Trace("Cells: {}", Cells);
            }
            for (int r=0; r<this.Cells.Count; r++)
            {
                for (int c=0; c<this.Cells[r].Count; c++)
                {
                    if (0 < this.Cells[r][c].Text.Length)
                    {
                        this.Cells[r][c].RowSpan = this.CellTable.GetZeroLenByRow(this.Cells, r, c);
                        this.Cells[r][c].ColSpan = this.CellTable.GetZeroLenByColumn(this.Cells, r, c);
                    }
                    Console.Write($"({this.Cells[r][c].RowSpan},{this.Cells[r][c].ColSpan})\t");
                }
                Console.WriteLine();
            }
            this.Cells = this.CellTable.StopColSpanByRowSpan(this.Cells);
            this.Cells = this.CellTable.StopRowSpanByColSpan(this.Cells);
            SetCrossSpan();
            this.Cells = this.CellTable.SetZeroRect(this.Cells);
            for (int r=0; r<this.Cells.Count; r++)
            {
                for (int c=0; c<this.Cells[r].Count; c++)
                {
                    Console.Write($"({this.Cells[r][c].RowSpan},{this.Cells[r][c].ColSpan})\t");
                }
                Console.WriteLine();
            }
        }
        private void SetCrossSpan()
        {
            foreach ((int r, int c) in this.CellTable.Cells(this.Cells))
            {
                if (1 < this.Cells[r][c].ColSpan)
                {
                    int C = CrossSpanRC(r, c, this.Cells[r][c].ColSpan);
                    if (-2 < C) { this.Cells[r][c].ColSpan = C; }
                }
            }
        }
        private int CrossSpanRC(int r, int c, int cs)
        {
            for (int C=c+1; C<c+cs; C++)
            {
                for (int R=r; 0<=R; R--)
                {
                    if (r <= this.Cells[R][C].RowSpan - 1 - R + r)
                    {
                        return C - c;
                    }
                }
            }
            return -2;
        }
        private void MakeReversedCells()
        {
            this.ReversedCells = this.CellTable.DeepCopy(this.Cells);
            this.ReversedCells.Reverse();
            this.Reverse();
        }
        private void Reverse()
        {
            Console.WriteLine("Reverse");
            for (int r=0; r<this.ReversedCells.Count; r++) { 
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].Text}\t");
                }
                Console.WriteLine();
            }
            for (int r=0; r<this.ReversedCells.Count; r++) { 
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].RowSpan},{this.ReversedCells[r][c].ColSpan}\t");
                }
                Console.WriteLine();
            }
            for (int r=0; r<this.ReversedCells.Count; r++)
            {
                for (int c=0; c<this.ReversedCells[r].Count; c++)
                {
                    if ("" == this.ReversedCells[r][c].Text)
                    {
                        SwapCell(r, c);
                    }
                }
            }
            for (int r=0; r<this.ReversedCells.Count; r++) { 
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].Text}\t");
                }
                Console.WriteLine();
            }
            for (int r=0; r<this.ReversedCells.Count; r++) {
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].RowSpan},{this.ReversedCells[r][c].ColSpan}\t");
                }
                Console.WriteLine();
            }
        }
        private void SwapCell(int r, int c)
        {
            for (int R=r+1; R<this.ReversedCells.Count; R++)
            {
                if ("" != ReversedCells[R][c].Text && 1 < ReversedCells[R][c].RowSpan)
                {
                    Cell tmp = ReversedCells[r][c];
                    ReversedCells[r][c] = ReversedCells[R][c];
                    ReversedCells[R][c] = tmp;
                    break;
                }
            }
        }
        /*
        public IEnumerable<List<Cell>> Cells()
        {
            foreach (var row in this.SourceList.GetRange(0, this.RowHeaderCount))
            {
                yield return row.GetRange(colCnt, row.Count - this.ColumnHeaderCount);
            }
        }
        */
        public void Infer(List<List<Cell>> SourceList)
        {

        }
        private int InferRowHeaderCount()
        {
            return 0;
        }
    }
    class ColumnHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        public CellTable CellTable { get; private set; }
        public int Count { get; private set; }
        public List<List<Cell>> Cells { get; private set; } = new List<List<Cell>>();
        public List<List<Cell>> ReversedCells { get; private set; } = new List<List<Cell>>();
        private int RowHeaderCount;
        private int ColumnHeaderCount;

        public ColumnHeader(GroupHeaderOptions opt, CellTable cellTable, int rowCnt, int colCnt)
        {
            this.Options = opt;
            this.CellTable = cellTable;
            this.RowHeaderCount = rowCnt;
            this.ColumnHeaderCount = rowCnt;
            logger.Debug("ColumnHeader");
            MakeCells(rowCnt, colCnt);
            if (ColumnHeaderPosType.r == this.Options.Column || 
                ColumnHeaderPosType.B == this.Options.Column) { MakeReversedCells(); }
        }
        private void MakeCells(int rowCnt, int colCnt)
        {
            foreach (var row in this.Options.SourceList.GetRange(rowCnt, Options.SourceList.Count - rowCnt))
            {
                this.Cells.Add(row.GetRange(0, colCnt));
            }
            for (int r=0; r<this.Cells.Count; r++)
            {
                for (int c=0; c<this.Cells[r].Count; c++)
                {
                    if (0 < this.Cells[r][c].Text.Length)
                    {
                        this.Cells[r][c].RowSpan = this.CellTable.GetZeroLenByRow(this.Cells, r, c);
                        this.Cells[r][c].ColSpan = this.CellTable.GetZeroLenByColumn(this.Cells, r, c);
                    }
                    Console.Write($"({this.Cells[r][c].RowSpan},{this.Cells[r][c].ColSpan})\t");
                }
                Console.WriteLine();
            }
            this.Cells = this.CellTable.StopColSpanByRowSpan(this.Cells);
            this.Cells = this.CellTable.StopRowSpanByColSpan(this.Cells);
            SetCrossSpan();
            this.Cells = this.CellTable.SetZeroRect(this.Cells);
            for (int r=0; r<this.Cells.Count; r++)
            {
                for (int c=0; c<this.Cells[r].Count; c++)
                {
                    Console.Write($"({this.Cells[r][c].RowSpan},{this.Cells[r][c].ColSpan})\t");
                }
                Console.WriteLine();
            }
        }
        private void MakeReversedCells()
        {
            logger.Debug("MakeReversedCells");
            this.ReversedCells = this.CellTable.DeepCopy(this.Cells);
            for (int r=0; r<this.ReversedCells.Count; r++)
            {
                this.ReversedCells[r].Reverse();
            }
            foreach (var row in this.ReversedCells)
            {
                foreach (var cell in row)
                {
                    Console.Write($"{cell.RowSpan},{cell.ColSpan}\t");
                }
                Console.WriteLine();
            }
            foreach (var row in this.ReversedCells)
            {
                foreach (var cell in row)
                {
                    Console.Write($"{cell.Text}\t");
                }
                Console.WriteLine();
            }
            this.Reverse();
        }
        private void SetCrossSpan()
        {
            foreach ((int r, int c) in this.CellTable.Cells(this.Cells))
            {
                if (1 < this.Cells[r][c].RowSpan)
                {
                    int R = CrossSpanCR(r, c, this.Cells[r][c].RowSpan);
                    if (-2 < R) { this.Cells[r][c].RowSpan = R; }
                }
            }
        }
        private int CrossSpanCR(int r, int c, int rs)
        {
            for (int R=r+1; R<r+rs; R++)
            {
                for (int C=c; 0<=C; C--)
                {
                    if (c <= this.Cells[R][C].ColSpan - 1 - C + c)
                    {
                        return R - r;
                    }
                }
            }
            return -2;
        }
        private void Reverse()
        {
            Console.WriteLine("Reverse");
            for (int r=0; r<this.ReversedCells.Count; r++) { 
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].Text}\t");
                }
                Console.WriteLine();
            }
            for (int r=0; r<this.ReversedCells.Count; r++) { 
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].RowSpan},{this.ReversedCells[r][c].ColSpan}\t");
                }
                Console.WriteLine();
            }
            for (int r=0; r<this.ReversedCells.Count; r++)
            {
                for (int c=0; c<this.ReversedCells[r].Count; c++)
                {
                    if ("" == this.ReversedCells[r][c].Text)
                    {
                        SwapCell(r, c);
                    }
                }
            }
            for (int r=0; r<this.ReversedCells.Count; r++) { 
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].Text}\t");
                }
                Console.WriteLine();
            }
            for (int r=0; r<this.ReversedCells.Count; r++) {
                for (int c=0; c<this.ReversedCells[r].Count; c++) {
                    Console.Write($"{this.ReversedCells[r][c].RowSpan},{this.ReversedCells[r][c].ColSpan}\t");
                }
                Console.WriteLine();
            }
        }
        private void SwapCell(int r, int c)
        {
            for (int C=c+1; C<this.ReversedCells[r].Count; C++)
            {
                if ("" != ReversedCells[r][C].Text && 1 < ReversedCells[r][C].ColSpan)
                {
                    Cell tmp = ReversedCells[r][c];
                    ReversedCells[r][c] = ReversedCells[r][C];
                    ReversedCells[r][C] = tmp;
                    break;
                }
            }
        }
    }
    class MatrixHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        /*
        public MatrixHeader(GroupHeaderOptions opt)
        {
            this.Options = opt;
        }
        */
        public MatrixHeader(GroupHeaderOptions opt, int rowCnt, int colCnt)
        {
            this.Options = opt;
        }

    }
}
