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
            ShowArgsConsole(opt);
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            Console.WriteLine("GroupHeader.ToHtml()");
            var cellTable = new CellTable(this.Options.SourceList);
            cellTable.SetBlankToZero();
            var header = new Header(this.Options);
            header.Infer(cellTable);
//            logger.Debug("InferColumnHeaderCount: {}", cellTable.InferColumnHeaderCount());
//            logger.Debug("InferRowHeaderCount: {}", cellTable.InferRowHeaderCount());
//            cellTable.InferColumnHeaderCount();
//            cellTable.InferRowHeaderCount();
            return "<table></table>";
        }
        private void ShowArgsConsole(GroupHeaderOptions opt)
        {
            Console.WriteLine("GroupHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine("Header: {0}", opt.Header);
            Console.WriteLine($"Row: {opt.Row}");
            Console.WriteLine($"Column: {opt.Column}");
            Console.WriteLine($"RowHeaderAttributes: {opt.RowHeaderAttributes}");
            Console.WriteLine($"ColumnHeaderAttributes: {opt.ColumnHeaderAttributes}");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
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
//                    logger.Debug("{}", C);
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
//                    logger.Debug("RC: {},{}", R, C);
                    if (r <= this.Cells[R][C].RowSpan - 1 - R + r)
                    {
//                        logger.Debug("C-c={}", C - c);
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
            ReversedText();
        }
        private void ReversedText()
        {
            Console.WriteLine("ReversedText");
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
                    /*
                    if ("" == this.ReversedCells[r][c].Text && 
                        0 < this.ReversedCells[r][c].RowSpan &&
                        0 < this.ReversedCells[r][c].ColSpan)
                    */
                    /*
                    if ("" == this.ReversedCells[r][c].Text && 
                        this.ReversedCells[r][c].RowSpan < 1 &&
                        this.ReversedCells[r][c].ColSpan < 1)
                    */
                    {
                        SwapText(r, c);
                    }
                }
            }
            foreach (var row in this.ReversedCells)
            {
                foreach (var cell in row)
                {
                    Console.Write($"{cell.Text}\t");
                }
                Console.WriteLine();
            }
        }
        private void SwapText(int r, int c)
        {
//            for (int R=r; R<this.ReversedCells.Count; R++)
            for (int R=r+1; R<this.ReversedCells.Count; R++)
            {
//                if ("" != ReversedCells[R][c].Text)
                if ("" != ReversedCells[R][c].Text && 1 < ReversedCells[R][c].RowSpan)
                {
                    string tmp = ReversedCells[r][c].Text;
                    ReversedCells[r][c].Text = ReversedCells[R][c].Text;
                    ReversedCells[R][c].Text = tmp;
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
        }
        private void SetCrossSpan()
        {
            foreach ((int r, int c) in this.CellTable.Cells(this.Cells))
            {
                if (1 < this.Cells[r][c].RowSpan)
                {
                    int R = CrossSpanCR(r, c, this.Cells[r][c].RowSpan);
//                    logger.Debug("{}", C);
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
//                    logger.Debug("RC: {},{}", R, C);
                    if (c <= this.Cells[R][C].ColSpan - 1 - C + c)
                    {
//                        logger.Debug("C-c={}", C - c);
                        return R - r;
                    }
                }
            }
            return -2;
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
